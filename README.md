# Summary of Project
## Summary
This is a project that I completed for a Webs Apps & Services assessment. For the assessment, I was provided with a Venues web service project and tasked with creating the Ecents web application and Catering web service.
### ThAmCo.Events
This is the main project of the three as this is the only MVC web application. This project is where everything is managed. You can create Events and reserve appropriate Venues and book Food Menus, create Customers/Staff and add them to Events, and more (all functionality is stated in the functionality section).
### ThAmCo.Venues
This is a web api service that was provided by an external source. This project is used to get the different Event Types, get available Venues for a given Event Type and Date, and to actually reserve and unreserve a Venue for a given Event.
### ThAmCo.Catering
This is a web api service that I created to handle all of the Catering tasks for an Event. This project allows you to perform CRUD operations (Create, Read, Update, and Delete) on Food Menus and also book and unbook Food Menus for an Event.

## Functionality
### MUST Functionality
| Feature                                                                                                                         | Info |
|---------------------------------------------------------------------------------------------------------------------------------|:----:|
| Create, list and edit Customers                                                                                                 | DONE |
| Create a new Event, specifying as a minimum its title, date and EventType                                                       | DONE |
| Edit an Event (except its date and type)                                                                                        | DONE |
| Book a Customer onto an Event as a Guest                                                                                        | DONE |
| List Guests (including a total count) for an Event and register their attendance;                                               | DONE |
| Display the details of a Customer, including information about the Events with which they are associated and their attendance   | DONE |
| Cancel the booking of a Customer from an upcoming Event                                                                         | DONE |
| Reserve an appropriate, available Venue for an Event via the ThAmCo.Venues web service, freeing any previously associated Venue | DONE |
| Display a list of Events that includes summary information about the Guests and Venue within it                                 | DONE |
### SHOULD Functionality
| Feature                                                                                                                                                                         | Info |
|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|:----:|
| Create, list and edit Staff                                                                                                                                                     | DONE |
| Adjust the staffing of an Event, adding available staff or removing currently assigned staff                                                                                    | DONE |
| See appropriate warnings within event list and staffing views when there is not a first-aider assigned to an Event                                                              | DONE |
| See appropriate warnings within event list and staffing views when there are fewer than one member of staff per 10 guests assigned to an Event                                  | DONE |
| Display the details of a Staff member, including information about upcoming Events at which they are assigned to work                                                           | DONE |
| Cancel (soft delete) an Event, freeing any associated Venue and Staff                                                                                                           | DONE |
| Display the details for an Event, which must include details of the Venue, Staff and Guests – this should be more detailed that the summary information found in the Event list | DONE |
| Permanently delete customer personal data by anonymising their Customer entity                                                                                                  | DONE |
### WOULD Functionality
| Feature                                                                                                                             | Info |
|-------------------------------------------------------------------------------------------------------------------------------------|:----:|
| Display a detailed list of available Venues , filtered by EventType and date range, and then create a new Event by picking a result | DONE |
| Create, list, edit and view the details of food Menus                                                                               | DONE |
| Book, edit and cancel Food for an Event                                                                                             | DONE |
| See appropriate information regarding Food for Events in the Event list and detail pages                                            | DONE |
| See a breakdown of costing for an Event in its details page.                                                                        | DONE |