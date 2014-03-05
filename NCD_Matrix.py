#!/usr/bin/env python3
# Creates a matrix using NCD with each row of a file.
import zlib
import os
import sys
import codecs
import csv

def compressionSize(stringIn):
    """Returns the binary length of the compression of stringIn
    ARGS:
        stringIn: string to be analyzed

    return:
        int: length of the string after it is compressed"""

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

    # Compress str1 and str2 ahead of time
    compressedStr1 = compressionSize(str1)
    compressedStr2 = compressionSize(str2)

    NCD_A = compressionSize(str1+str2)
    NCD_B = min(compressedStr1, compressedStr2)
    NCD_C = max(compressedStr1, compressedStr2)

    NCD_result = (NCD_A - NCD_B) / NCD_C #This is the formula that is used
    return NCD_result

def main():
    return()

if __name__ == '__main__':
    in_filename = input("Please enter a filename for read: ")
    out_filename = input("Please enter a filename for results: ")

    print("Opening the file for read")
    with codecs.open(in_filename, 'r') as f:
        lines = f.read().splitlines()
    print("Done")

    print("Calculating NCD")
    total_lines = len(lines)

    output = []
    for i, row in enumerate(lines):
        output.append([])
        for column in lines:
            output[i].append(getNCD(row, column))
        print("Row:[" + str(i) + "] out of [" + str(total_lines) + "]")

    print("Done")
    

    print("Writing to file:")
    with open(out_filename, 'w') as fout:
        writer = csv.writer(fout)
        for row in output:
            writer.writerow(row)

    print("Done")

    main()
