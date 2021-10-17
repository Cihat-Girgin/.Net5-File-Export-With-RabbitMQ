# # .Net5-File-Export-With-RabbitMQ

This app shows how client and file creation service communicate with RabbitMQ.
In our scenario, the user will get the excel output of the companies table in the database.

First user saves the file name to the database by triggering as follows. And meanwhile, using direct exchange on the RabbitMQ side, the resulting fileId is sent to the queue.

![excelgif](https://user-images.githubusercontent.com/73026903/137640496-e4019696-86e8-46fd-8d20-432ad519be6f.gif)

Our running service (consumer) takes the fileID and creates an excel file according to this ID. 
In the next step, our service sends the resulting file to the UploadController for uploading in our web project (producer) and the incoming file is saved in the wwwroot folder in our web project.

And finally, with the help of SignalR, the user is informed that the process has ended.

![rabbitR](https://user-images.githubusercontent.com/73026903/137641082-c570f736-a140-4a99-9d74-abbe31be262c.png)

## Installation
First, update the connection information in the appsettings.json file in the web project and in the service.
```
  "ConnectionStrings": {
    "SqlServer": "Data Source=DESKTOP-77K8SPT\\SQLEXPRESS;Initial Catalog=RabbitMqDb;User ID=<Username>;Password=<Password>;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
  },
  "RabbitMQ": "Your AMQP URL ",
```

Then run the following command in the package manager console of the web project to mirror the migration files to the database.

```sh
update-database
```
500 company objects will be saved in the database when the application first starts up(via Faker.Net)



##  Advice

First run the web project(Producer, exchange and queue being creating on this step)
After run the worker service.(Consumer)

## License

MIT

**Good luck guys!**
