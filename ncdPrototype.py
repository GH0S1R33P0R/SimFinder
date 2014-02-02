#!/usr/bin/env python3
# ncdPrototype.py
# Basic prototype of using Normalized Compression Distance to compare two strings.
import zlib
import csv
import os
import sys


#TODO (Bader): Add level as param for zlib.compress
def compressionSize(stringIn):
    """Returns the binary length of the compression of stringIn

        Input:
            stringIn: string to be analyzed
        Output:
            int: length of the string after it is compressed
    """

    #TODO (Bader): take in binary input from file
    bytesIn = bytes(stringIn, 'utf-8')

    compressed = zlib.compress(bytesIn)
    length = len(compressed)
    return length

def getNCD(str1, str2):
    """Returns the Normalized compression distance of two strings

        Input:
            str1, str2: The two strings to be compared
        Output:
            int: The distance according to NCD
    """
    NCD_A = compressionSize(str1+str2)
    NCD_B = min(compressionSize(str1), compressionSize(str2))
    NCD_C = max(compressionSize(str1), compressionSize(str2))

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
        next(csvReader, None)

        items = [] #Holder for items I need in the csv
        for row in csvReader:
            items.append(row)
        return(items)

def runComparisonOnItems(listOfItems):
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
    

def main():
    runMode = int(
            input("---ncdPrototype---\n"
                + "1) Generic 1 file run\n"
                + "2) Generic 2 file run\n"
                + "3) Generic 3 file run\n")
            )

    print(runMode)

    if (runMode == 1):
        fileName = input("Please enter a filename:")
        rowList = GetRowsFromCSV(fileName)
        runComparisonOnItems(rowList)
    elif (runMode == 2):
        fileName1 = input("Please enter a filename:")
        fileName2 = input("Please enter another filename:")

        rows1 = GetRowsFromCSV(fileName1)
        rows2 = GetRowsFromCSV(fileName2)




    elif (runMode == 3):
        print("Still working on this")
        #TODO: Selective two file run
    """
    else:

    """

if __name__ == '__main__':
    main()
