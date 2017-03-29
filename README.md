Description
==================

A Library to aid in the finding of similar tickets.
This library uses the [Normalized Compression Distance](http://en.wikipedia.org/wiki/Normalized_compression_distance)  to compare objects against each other.

The code for this library is split up into sub-projects for ease of use and maintainability.


SimFinder
=================
Contains interfaces for the functionality.

ICompressible: Object to be compressed

|Member | Description|
|------|------------|
| ToByteArray() | Returns a byte[] representation of the entity |
| Complexity    | Stored complexity value |

ISimililarity : Interface for object used to perform similarity calculation

|Member | Description|
|------|------------|
| GetComplexity(ICompressible entity) | Returns the complexity of the ICompressible Object passed in |
| SetComplexity(ICompressible entity) | Sets the complexity of the ICompressible Object passed in |
| GetSimilarity(ICompressible entity1, ICompressible entity2) | Returns a real number representing the similarity between entity1 and entity2 |
|  IsSimilar(ICompressible entity1, ICompressible entity)  | Returns true if two entitys are similar | 
| Threshold | What level of similarity should two ICompressibles be before we define them to be similar |
| FindSimilarEntities(ICompressible entity, ICompressible[] dataSet) | Return a list of similar ICompressible Entities to a single ICompressible |


SimFinderUtils
=================
Our own implementation to the Interfaces that we have defined.
These classes should be re-written to match the data of your own implementation if needed.

|Member | Description|
|-------|------------|
|Similarity.cs         | A comparison class implementing ISimiliarity using NCD |
|StringCompressible.cs | Compressible class for Strings |
|TicketCompressible.cs | Compressible class for IT-Support Tickets |

WebDemo
=================

An ASP.net based demonstration of this library in use.
