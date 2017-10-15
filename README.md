# Refaction Improved, by Struan Judd

## Changes
1. Retained Instructions in own document
1. Semantic Versioning of assemblies
2. Created basic acceptace tests
3. Begun Questions document
4. Capture Original DB
5. Separate Model into own assembly/project. No functional changes.
   1. First Unit Test
   2. Refactor controller GetAll to use service
   3. Refactor controller SearchByName to use service
   4. Refactor controller GetProduct to use service
   5. Refactor controller Create to use service
   6. Refactor controller Update to use service
   7. Refactor controller Delete to use service
   8. Refactor All Product Option Controller methods to use Options service
   9. Refactor for readability
   
## Functional tests
These are written in Python and run using Nose (eg. `py -m nose`)

To install:
1. Install Python 3.6
2. Run `pip install -r requirements.txt`

They assume that the Web Api is running at a base URL of `http://localhost:58123`
