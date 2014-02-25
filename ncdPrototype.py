#!/usr/bin/env python3
#-*- coding: utf-8 -*-
# ncdPrototype.py
# Basic prototype of using Normalized Compression Distance to compare two strings.
import zlib
import csv
import os
import sys
import operator
import webbrowser
import re
#Fixes UTF-8 issues in Windows
import codecs

#GLOBALS
allColumns = None
main_OID_column = None
file_list = None

OID_Matches = None
Ticket_Matches = None
sizeOfFiles = None
SERVERNAME = None


#TODO (Bader): Add level as param for zlib.compress
def compressionSize(stringIn):
    """Returns the binary length of the compression of stringIn
    ARGS:
        stringIn: string to be analyzed

    return:
        int: length of the string after it is compressed"""

    #TODO (Bader): take in binary input from file
    bytesIn = bytes(stringIn, 'utf-8')

    compressed = zlib.compress(bytesIn)
    length = len(compressed)
    return length

def getNCD(str1, str2):
    """Returns the Normalized compression distance of two strings
    ARGS:
        str1, str2: The two strings to be compared

    return:
        int: The distance according to NCD"""

    #Compress str1 and str2 ahead of time
    compressedStr1 = compressionSize(str1)
    compressedStr2 = compressionSize(str2)

    NCD_A = compressionSize(str1+str2)
    NCD_B = min(compressedStr1, compressedStr2)
    NCD_C = max(compressedStr1, compressedStr2)

    NCD_result = (NCD_A - NCD_B) / NCD_C #This is the formula that is used
    return NCD_result

def GetRowsFromCSV(fileName):
    """ Function to get the values from a CSV file
    ARGS:
        fileName: name of the file to read
        be delimeted by ','

    return:
        list of rows from the file. """

    #Opening the file as a csv.reader object
    with open(fileName, 'r') as csvFile:
        csvReader = csv.reader(csvFile)
        columns = next(csvReader, None)

        #print("\nColumns of " + fileName + " are:")
        #for idx, column in enumerate(columns):
        #    print(str(idx) + ") " + str(column))

        items = [] #Holder for items I need in the csv
        for row in csvReader:
            items.append(row)
        return(items)

def runComparisonOnItemsAgainstSelf(listOfItems):
    """Run getNCD for each item gainst each other and output the results
    ARGS:
        listOfItems: a list of items to run comparison on

    return:
        list of rows where each row represents an item, 
        and the column represents the result of comparison with another item"""

    for i in listOfItems:
        for j in listOfItems:
            result = getNCD(str(i), str(j))
            print(result, end=",")
        print()

def compareAllAgainstSummaryAndComments(listOfItems, summary,  comments):
    """Run getNCD for each item gainst one ticket and output the results
    ARGS:
        listOfItems: a list of items to run comparison on
        summary: a summary string to use for the ticket
        comments: a comment string to use for the ticket

    return:
        list of rows where each row represents an item, 
        and the column represents the result of comparison with another item"""

    ItemToCompare = [summary, comments]

    listToSort = []
    for i in listOfItems:
        result = getNCD(str(i), str(ItemToCompare))
        listToSort.append([result, i])

    #Sort list by NCD value
    sortedList = sorted(listToSort, key=operator.itemgetter(0))

    return(sortedList)

def compareAllAgainstItemID(listOfItems, ItemID):
    """Run getNCD against a single ItemID
    ARGS:
        listOfItems: a list of items to run comparison on
        ItemID: ID of ticket in system (Matches OID)

    return:
        list of rows where each row represents an item, 
        and the column represents the result of comparison with another item"""

    ID_Stripped = int(ItemID[3:]) #Essentially  /^IR-0*(.*)/

    found = None

    for i, item in enumerate(listOfItems):
        if(int(item[0]) == ID_Stripped):
            found = item

    if(found == None):
        print("ID Does not exist!")
        exit(1)


    listToSort = []
    for i in listOfItems:
        result = getNCD(str(i), str(found))
        listToSort.append([result, i])

    #Sort list by NCD value
    sortedList = sorted(listToSort, key=operator.itemgetter(0))

    return(sortedList)




def combineCSVsSelectively(CSV1, CSV2):
    """Combines two list of rows with matching OID values from Ticket_Matches
    ARGS:
        CSV1: First list of rows 
        CSV2: Second list of rows 

    return:
        list of rows where the OID matches"""

    #row2 can have multiple instances, append all to single row1
    outputCSV = []
    for row1 in CSV1:
        OID = row1[main_OID_column]
        temp = [OID]
        temp = temp + [row1[Ticket_Matches[0]]] #Grab summary
        for row2 in CSV2:
            if row2[OID_Matches - sizeOfFiles[0]] == OID:
                #Grab comments from second file
                temp = temp + [row2[Ticket_Matches[1] - sizeOfFiles[0]]]
        outputCSV.append(temp)
    return(outputCSV)

def combineCSVs(CSV1, CSV2):
    """Combines two list of rows with matching OID values
    ARGS:
        CSV1: First list of rows 
        CSV2: Second list of rows 

    return:
        list of rows where the OID matches"""

    #row2 can have multiple instances, append all to single row1
    outputCSV = []
    for row1 in CSV1:
        OID = row1[0]
        temp = row1
        for row2 in CSV2:
            if row2[26] == OID:
                temp = temp + row2
        outputCSV.append(temp)
    return(outputCSV)

def selectSummaryAndCommentsColumns(CSVin):
    """Outputs a list af rows from combineCSV
        with only summary and comments fields
    ARGS:
        CSVin: Output of combineCSVs

    return:
        List of rows with just summary and conclusion"""

    # This is hardcoded. 
    #TODO(Bader) Grab first line from CSV to obtain columns
    outputCSV = []
    for row in CSVin:
        outputCSV.append([row[16], row[86+6]])
    return outputCSV

def selectOIDSummaryAndCommentsColumns(CSVin):
    """Outputs a list af rows from combineCSV
        with only summary and comments fields
    ARGS:
        CSVin: Output of combineCSVs

    return:
        List of rows with just summary and conclusion"""

    # This is hardcoded. 
    #TODO(Bader) Grab first line from CSV to obtain columns
    outputCSV = []
    for row in CSVin:
        outputCSV.append([row[0], row[16], row[86+6]])
    return outputCSV

def main():

    global allColumns
    global main_OID_column
    global file_list
    global OID_Matches
    global Ticket_Matches
    global sizeOfFiles
    global SERVERNAME

    #Ask if want to open files in repl
    main_file = input("Please enter the main file:")

    allColumns = []
    sizeOfFiles = []

    #TODO(Bader): Make this try-except and forloop  into a function
    try:
        with open(main_file, 'r') as f:
            columns = f.readline().strip().split(',')
    except IOError:
        print("Error: file does not exist")
        return
    sizeOfFiles.append(len(columns))

    for i, column in enumerate(columns):
        column_with_fileName = main_file + "." + column
        allColumns.append(column_with_fileName)
        print(str(i) + ")["  + column_with_fileName + "]")

    #Get the OID column to match everything else with
    main_OID_column = int(input("Enter the column with the OID:"))

    #Create a list of all files
    file_list = [main_file]

    #REPL for getting other files
    print("***Getting Other Files***")
    while True:

        #break condition for loop
        #another_file = input("Enter another file (Y/n)?:").lower()
        #if(another_file == 'n'):
        #    break

        file_list.append(input("File name:"))
        break #TODO: Do we NEED more than two files?


    #Getting a combined list of columns
    for fileName in file_list[1:]:
        print("For file [" + fileName + "] ")
        try:
            with open(fileName, 'r') as f:
                columns = f.readline().strip().split(',')
        except IOError:
            print("Error: file does not exist")
            return
        sizeOfFiles.append(len(columns))

        for i, column in enumerate(columns):
            column_with_fileName = fileName + "." + column
            allColumns.append(column_with_fileName)

#        next_file_OID_num = int(input("Which column has the ID value?")) 

#        column_list.append(next_file_OID_num)


    for i, column in enumerate(allColumns):
        print(str(i) + ")[" + column + "]")

    OID_Matches = int(input("Enter column to match OID with:"))
#                        "(seperate with commas):")
#    OID_Matches = list(map(int, OID_Matches.strip().split(',')))

    Ticket_Matches = input("Enter columns to match the ticket with"
#                        "(seperate with commas):")
                            "(Summary, Comments):")
    Ticket_Matches = list(map(int, Ticket_Matches.strip().split(',')))


    SERVERNAME = input("What is the Server name?:")
    CSV1 = GetRowsFromCSV(file_list[0])
    CSV2 = GetRowsFromCSV(file_list[1])
    print("Combining the files")
    combinedCSV = combineCSVsSelectively(CSV1, CSV2)
    #TODO(Bader): select columns from combinedCSV

    URLPrefix = "http://" + SERVERNAME + "CGWeb/MainUI/ServiceDesk/SDItemEditPanel.aspx?boundtable=IIncidentRequest&ID="


    stillRunning = True

    while(stillRunning):
        print("*****************")
        print("What input type")
        runMode = int(input("\t0)ItemID or"
                            "\n\t1)Sample ticket"
                            "\n\t*)Quit:\n"))

        if (runMode == 0):
            ItemID = input("What is the ItemID:")
            sortedList = compareAllAgainstItemID(combinedCSV,ItemID)
        elif (runMode == 1):
            Summary = input("Please enter a summary string:")
            Comments = input("Please enter a comment string:")
            sortedList = compareAllAgainstSummaryAndComments(combinedCSV, Summary, Comments)
        else:
            stillRunning = False
            break

        for i, item in enumerate(sortedList[0:20]): #Change that number?
            print(str(i) + ") " + "\t".join(map(str,item)))

        toOpen = int(input("Which one do you want to open?"))
        if (toOpen > -1):
            webbrowser.open(URLPrefix + str(sortedList[toOpen][1][0]))


    #TODO(Bader): Ask if ToUpper (or lower) is needed
    #TODO(Bader): Ask if NULL should be stripped
    #TODO(Bader): All below is a repl
    #TODO(Bader): For each column, ask for a string
    #TODO(Bader): Return top x results
    #TODO(Bader): Ask for i in x to open in browser?
    #TODO(Bader): openURL

"""
    if (runMode == 1):
        fileName = input("Please enter a filename:")
        rowList = GetRowsFromCSV(fileName)
        runComparisonOnItemsAgainstSelf(rowList)

    elif (runMode == 2):
        fileName1 = input("Please enter a filename:")
        fileName2 = input("Please enter another filename:")

        CSV1 = GetRowsFromCSV(fileName1)
        CSV2 = GetRowsFromCSV(fileName2)
        combinedCSV = combineCSVs(CSV1, CSV2)
        runComparisonOnItemsAgainstSelf(combinedCSV)

    elif (runMode == 3):
        fileName1 = input("Please enter a filename:")
        fileName2 = input("Please enter another filename:")

        CSV1 = GetRowsFromCSV(fileName1)
        CSV2 = GetRowsFromCSV(fileName2)
        combinedCSV = selectSummaryAndCommentsColumns(combineCSVs(CSV1, CSV2))
        runComparisonOnItemsAgainstSelf(combinedCSV)
    elif (runMode == 4):
        fileName1 = input("Please enter a filename:")
        fileName2 = input("Please enter another filename:")

        CSV1 = GetRowsFromCSV(fileName1)
        CSV2 = GetRowsFromCSV(fileName2)
        combinedCSV = selectOIDSummaryAndCommentsColumns(combineCSVs(CSV1, CSV2))

        while True:
            Summary = input("Please enter a summary string:")
            Comments = input("Please enter a comment string:")
#TODO(Bader): summary and comments toupper. Do same for above
            compareAllAgainstSummaryAndComments(combinedCSV, Summary, Comments)
            """



if __name__ == '__main__':
    main()
