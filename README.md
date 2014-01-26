USFSpr2014SunView
==================
Working repository for SunView group in USF's Senior Project Spring 2014.


Project Requirements
==================
The final project should have the following requirements fulfilled.

1. Produce a confidence rating between two tickets. 

    1.1 The system should be able to produce the results in a reasonable amount of time that scales linearly with the number of tickets that must be searched. The minimum acceptable would be one second for every thousand tickets to be searched.
    
    1.2 From a collection of tickets, produce a ranked list of matching tickets sorted by confidence rating, it should generally be detectable by a user why the ticket was considered similar and why a given ticket ranked higher than another
2. Be written in C#, or another .net compatible language. 
3. Be capable of performing generic comparisons. 

    3.1 Can work on non-text fields in a ticket.
4. Have calculations pre-computed and stored, if possible.

There are also two advanced (optional) requirements for this project:

1. Provide a method for prioritizing speed vs. space.
2. Provide a method for clustering similar tickets.

