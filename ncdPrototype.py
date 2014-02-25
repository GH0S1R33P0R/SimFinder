#!/usr/bin/env python3
#-*- coding: utf-8 -*-
# ncdPrototype.py
# Basic prototype of using Normalized Compression Distance to compare two strings.
import zlib
import csv
import os
import sys
import operator
#Fixes UTF-8 issues in Windows
import codecs


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

        print("\nColumns of " + fileName + " are:")
        for idx, column in enumerate(columns):
            print(str(idx) + ") " + str(column))

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

    sortedList = sorted(listToSort, key=operator.itemgetter(0))

    for i in sortedList[0:20]:
        print(",\t".join(map(str,i)))

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
    runMode = int(
            input("---ncdPrototype---\n"
                + "1) Generic 1 file run\n"
                + "2) Generic 2 file run\n"
                + "3) Specific 2 file run\n"
                + "4) Test user input with 2 files\n"
                )
            )

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



if __name__ == '__main__':
    main()
