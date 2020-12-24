# RESTfulAPI
A RESTful API application built using Visual Studio, ASP.NET, C#, SQL.

### Project Goal

The goal of this project was to create a simple RESTful Application with CRUD *(Create, Read, Update, Delete)* functionalities so that I could get a better understanding of how APIs work and the range of methods and syntaxes that are required in order to do so.

#### Class Diagram

The class diagram below provides a quick overview of the two main controllers that were used. By looking at the image you will notice that one of the controllers was refactored to include a service layer. The reason behind this was to implement the **D** in the SOLID principles (Dependency Inversion) as well as to enable the use of **Mocks** to test the class. Due to time constraint the second controller was not fully refactored, however having gained a better understanding from the first controller the refactoring of the second controller was not set as one of the major tasks to complete. 

![classDiagram](https://github.com/sarkerJ/RESTfulAPI/blob/main/Classdiagram.JPG)

#### Postman Requests

Both controllers were tested using the Postman application to ensure that all of the CRUD functionalities were working as intended. Below you can see an example of the 4 requests that were executed for the "PersonController" class. 

![PersonTest](https://github.com/sarkerJ/RESTfulAPI/blob/main/PostmanTest.JPG) 



#### MockTests

Mock tests were used to further ensure that all of the methods in the controllers were working as intended. Mock testing has a range of benefits, one of which is to allow developers to isolate a particular class and test it without having to worry of the external dependencies the class may be dependent affecting the test results.

![TodoTest](https://github.com/sarkerJ/RESTfulAPI/blob/main/MockTest.JPG)