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
            length of the string after it is compressed
    """
    #TODO (Bader): take in binary input from file
    bytesIn = bytes(stringIn, 'utf-8')
    compressed = zlib.compress(bytesIn)
    length = len(compressed)
    print("Length is: "  + str(length))


def main():
    str1 = ''
    str2 = ''

    str1 = input("Enter the first string:")
    str2 = input("Enter the second string:")

    print("The first string is: " + str1)
    print("The second string is: " + str2)

    compressionSize(str1)

if __name__ == '__main__':
    main()
