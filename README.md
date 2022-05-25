# PSU Payment Gateway
This is a demo payment provider for the Bachelor in Software Development at University College Nordjylland. The purpose is to learn how to integrate different systems and as such this payment provider is a part of a larger setup.
This is a ASP.NET Core Web API solution with Swagger for documentation. The `Dockerfile` will build this solution in release mode, meaning that there are no swagger in that solution.

It is possible to throttle the solution to limit how many requests it is able to handle. This is managed in the `appsettings.json` file.

There are some limitations on this implementation. The first is the throttling, the second is that the payment gateway will not accept dublicate payments (cardnumber and amount) and will report an error. This is saved in memory, so just restart the service for the changes to take effect.

## Using Docker
If you prefer to get the payment gateway up and running as a docker image, you may access it on `hub.docker.com` or run it directly with this command.

    docker run -d -p 8080:80 brhv/psupaymentgateway:latest
    
You can now access the gateway on: `http://localhost:8080/swagger`: *Please note* You must include `/swagger` else you will get the generic page, that the resource can not be found.

## Enable logging
This service is prepared to be used with the logging provider `Seq` [External link to Seq](https://datalust.co/seq). In the config file you can change the location of the `Seq` provider. If you are interested in running Seq as a local service use a docker image:

    docker run --name seq -d --restart unless-stopped -e ACCEPT_EULA=Y -p 8080:80 -p 5341:5341 datalust/seq:latest
    
Please note that if the gateway is not on the same network as the Seq server then they cannot communicate (typically when the payment gateway is running in Docker). To see the logs navigate to: `http://localhost:8080`.
