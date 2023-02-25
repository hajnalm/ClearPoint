# ClearPoint Tech Test

This repository contains the ClearPoint tech test code with **Marton Hajnal**'s changes.


# Backend changes
## Repository pattern
It's a good practice to separate the data persistence logic from the Controller class. For this reason, I've created the **ITodoRepository** interface, and its **TodoRepository** implementation.
This change also allows for more granular testing, as the **TodoItemsController** can now be tested with a mock **ITodoRepository**.

## DTO's
When sending and receiving data through a Web API method, it's generally a good idea to use data transfer objects that are decoupled from the data context entities. The **TodoItemReadDto** and **TodoItemWriteDto** classes serve this purpose. Although in this case they look almost identical to the **TodoItem** class, in many real life scenarios, the DTO's can have different properties than the class that represents the data context entity.
To facilitate conversion between the DTO's and **TodoItem**, I've added an AutoMapper profile called **TodoProfile**.

