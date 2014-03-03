#!/usr/bin/env python3
# Merges two CSV files based on OID value
import codecs
import csv
import os

_TOTAL_COLUMNS_NUM = [] # Total number of columns
_ALL_COLUMNS = []

def open_file_and_list_rows(file_name):
    """Obtains the list of rows in a file. Prints each item to the screen
    ARGS:
        file_name: A string representing the file to be opened
    return: 
        A list of strings where each string is a colum
    """
    global _TOTAL_COLUMNS_NUM
    global _ALL_COLUMNS

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
        _ALL_COLUMNS.append(column_with_fileName)
        print(str(i) + ")\t["  + column_with_fileName + "]")
    return columns

def get_rows_from_csv(fileName):
    """ Function to get the values from a CSV file
    ARGS:
        fileName: name of the file to read
        be delimeted by ','

    return:
        list of rows from the file. """

    #Opening the file as a csv.reader object
    with codecs.open(fileName, 'r', 'utf-8') as csvFile:
        csvReader = csv.reader(csvFile)
        columns = next(csvReader, None)

        #print("\nColumns of " + fileName + " are:")
        #for idx, column in enumerate(columns):
        #    print(str(idx) + ") " + str(column))

        items = [] #Holder for items I need in the csv
        for row in csvReader:
            items.append(row)
        return(items)
def combine_all_ticket_values(CSV1, CSV2, OID_1, OID_2, Item_ID_column):
    """Combines two list of rows with matching OID values from Ticket_Matches
    ARGS:
        CSV1: First list of rows 
        CSV2: Second list of rows 
        OID_1: Column number for first CSV's OID
        OID_2: Column number for second CSV's OID

    return:
        list of rows where the OID matches"""

    #row2 can have multiple instances, append all to single row1
    outputCSV = []
    for row1 in CSV1:
        OID = row1[OID_1] # Saving the OID value of the ticket
        temp = [row1[Item_ID_column]] # For saving the ItemID
        temp = temp + row1 # Add the row from the first file
        for row2 in CSV2: # Adding all relevant rows from second file
            if row2[OID_2] == OID:
                # Add the history data
                temp = temp + row2
        #TODO: Strip NULL and toUpper all elements in temp
        #Removing all NULL's
      #  temp = [item for item in temp if item != 'NULL']
        outputCSV.append(temp)

    # Debug stuff
    for i in outputCSV[:3]:
        print(i)
    return(outputCSV)

def main():
    # Getting the information about the main file
    main_file = input("Please enter the main file: ")
    open_file_and_list_rows(main_file)

    # Get the OID column to match everything else with
    main_OID_column = int(input("Enter the column with the OID: "))
    main_Item_ID_column = int(input("Enter the column with the ItemID: "))

    # Getting information about the second file
    second_file = input("Please enter the second file: ")
    open_file_and_list_rows(second_file)
    second_OID_column = int(input("Enter the column with the OID: "))

    # Combining the data of the two CSV files
    print("Extracting data from files")
    CSV1 = get_rows_from_csv(main_file)
    print("File:[%s] is done" % (main_file))
    CSV2 = get_rows_from_csv(second_file)
    print("File:[%s] is done" % (second_file))


    print("Combining the files")
    combinedCSV = combine_all_ticket_values(CSV1, CSV2,\
            main_OID_column, second_OID_column, main_Item_ID_column)


if __name__ == '__main__':
    main()
