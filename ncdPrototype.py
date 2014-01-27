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
#    print("Arg in is: [" + stringIn + "] with length: " + str(length))
    return length


def main():
    str1 = ''
    str2 = ''

    str1 = input("Enter the first string:")
    str2 = input("Enter the second string:")

#   print("The first string is: " + str1)
#   print("The second string is: " + str2)

    #TODO (Bader): Make this into a function
    NCD_A = compressionSize(str1+str2)
    print("NCD_A: " + str(NCD_A))
    NCD_B = min(compressionSize(str1), compressionSize(str2))
    print("NCD_B: " + str(NCD_B))
    NCD_C = max(compressionSize(str1), compressionSize(str2))
    print("NCD_C: " + str(NCD_C))

    NCD_result = (NCD_A - NCD_B) / NCD_C

    print("The NCD result is: " + str(NCD_result)) 

if __name__ == '__main__':
    main()
