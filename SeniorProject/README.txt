
SimFinder:
    Main Library, contains interfaces for the functionality.
        ICompressible: Interface for object to be compressed.
            ToByteArray: Returns a byte array representing the object.
            Complexity: An int representing how complex this object is.
        ISimililarity: Interface for object used for similarity calculation.
            GetComplexity: returns the complexity of the ICompressible Object passed in.
            SetComplexity: sets the complexity of the ICompressible Object passed in.
            GetSimilarity: Returns a value to represent how similar two ICompressible objects are.
            IsSimilar:  Returns true two objects' similarity is better than the threshold.
            Threshould: What level of similarity should two ICompressibles be before we define them to be similar.
            FinSimilarEntities: Return a list of similar ICompressible Entities to a single ICompressible.
Utils:
    Implementations to the Interfaces in SeniorProject.
WebDemo:
    A web demonstration to show the library in action.
    For this project to function, IncidentRequest.csv or a similar file needs to be in ~/App_Data.
Tests:
    This is where Tests for the library are placed.
Analytics:
    A test driver application.
    


