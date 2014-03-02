#!/usr/bin/env python3
# Merges two CSV files based on OID value
import codecs
import os

_TOTAL_COLUMNS_NUM # Total number of columns

def open_file_and_list_rows(file_name):
    """Obtains the list of rows in a file. Prints each item to the screen
    ARGS:
        file_name: A string representing the file to be opened
    return: 
        A list of strings where each string is a colum
    """
    global TOTAL_COLUMNS_NUM

    try:
        with codecs.open(file_name, 'r', 'utf-8') as f:
            # columns is a list of each column in the CSV
            columns = f.readline().strip().split(',')
    except IOError:
        print("Error: file does not exist")
        exit(-1)


    _TOTAL_COLUMNS_NUM.append(len(columns))

    for i, column in enumerate(columns):
        column_with_fileName = file_name + "." + column
        allColumns.append(column_with_fileName)
        print(str(i) + ")["  + column_with_fileName + "]")
    return columns


