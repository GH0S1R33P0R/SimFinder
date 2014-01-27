#!/usr/bin/env python3
# ncdPrototype.py
# Basic prototype of using Normalized Compression Distance to compare two strings.
import zlib
import os

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

def main():
    str1 = ''
    str2 = ''

    str1 = input("Enter the first string:")
    str2 = input("Enter the second string:")

    distance = getNCD(str1, str2)
    print("Normalize compression distance is: " + str(distance))


if __name__ == '__main__':
    main()
