# ClearPoint Tech Test

This repository contains the ClearPoint tech test code with **Marton Hajnal**'s changes.


# Backend changes
## Repository pattern
It's a good practice to separate the data persistence logic from the Controller class. For this reason, I've created the **ITodoRepository** interface, and its **TodoRepository** implementation.
This change also allows for more granular testing, as the **TodoItemsController** can now be tested with a mock **ITodoRepository**.

## DTO's
When sending and receiving data through a Web API method, it's generally a good idea to use data transfer objects that are decoupled from the data context entities. The **TodoItemReadDto** and **TodoItemWriteDto** classes serve this purpose. Although in this case they look almost identical to the **TodoItem** class, in many real life scenarios, the DTO's can have different properties than the class that represents the data context entity.
To facilitate conversion between the DTO's and **TodoItem**, I've added an AutoMapper profile called **TodoProfile**.

## Updated Controller methods
Separating the repository logic from the Controller means that the Controller's methods had to be refactored.
In addition to those changes, the method signatures were updated to indicate what kind of object(s) are expected to be returned, instead of just a plain nondescript **IActionResult**. 
The **id** parameter was removed from the input parameter list of the **PutTodoItem(..)** method. The id is present in the **todoItem** parameter, and since PUT methods should be idempotent anyway, it would be redundant.
Also added **ProducesResponseType** attributes for an improved Swagger output.

## XUnit test
I've added a few example unit tests for the **TodoItemsController** class. Note: these are just examples and for a real commercial project I would add significantly more tests to provide a good code coverage.

## Documentation
I wrote documentation for the most important classes and methods. 


# Frontend
At the current stage I haven't done any work on the frontend code. I'm happy to do the listed tasks once ClearPoint has an actual role that I would like to apply for.
