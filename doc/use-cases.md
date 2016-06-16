# Use Cases

MAGES is designed to be placed in *any* .NET or Unity application. It fits perfectly as a glue between loosely coupled components, as the base for a plugin mechanism (running sandboxed code), or to provide in interactive shell within the application.

## Interactive Scripting

MAGES is a handy helper to enable interactive scripting in console, Windows Forms, WPF, or even web applications. There are multiple ways how we can integrate MAGES to achieve the desired behavior. We could

* have a method of entering arbitrary input that is evaluated by MAGES
* run the application with a startup script provided by the user
* let the user set some handlers for certain actions
* provide a configuration that is loaded to run under certain conditions

to mention a few. 

## Coupling Components

It is known since a while that only loosely-coupled components allow scaling and flexibility. Having an `INotificationService` as a dependency to a class allows the class to be used together with an `EmailNotificationService`, a `SmsNotificationService`, and any other object implementing this interface. The class is therefore loosely coupled to the classes implementing the `NotificationService`. But a third class has to know that these implementations exist (or at least one of them). This is the glue.

We usually tend to have a kind of binder that is working, e.g., via reflection, to avoid strong-coupling. Nevertheless, this fixes it in some other areas. For instance, we may now be constraint by the contents of a library, which cannot change on the fly. Using MAGES as a library to execute code (which may be written in a config file or defined freely in any text, see scripting) could solve this problem. Now MAGES uses the exposed classes and / or objects to bring them together in dynamic ways.

The idea is simple: Write small (highly performing, critical) modules that can be used together in arbitrary ways. Leave the coupling to a dynamic component that does not know anything about the modules in any way, but can bring them together.

## Sandbox Code

Let's say we have a Windows Forms application that contains some financial data. The user may want to run complex queries on the data. Besides presenting a general form that has the ordinary pilot in a cockpit feeling we may want to provide some flexibility. We therefore use the form to generate some MAGES code. Furthermore, we also provide the possibility to enter such code in a text field (hence skipping the complex form).

The idea is that MAGES will be used as a sandbox that can only access the financial data. We generate a MAGES function that is responsible for the transformation. In the end we check the code for validity, compile the transformation function, and invoke it for every item in the set.