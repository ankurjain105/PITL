# PITL
1. Have used Topshelf nuget package to ease creation of windows service. Primary advantage being it is easy to test locally as a console app and can be installed as windows service using 
   Petro.PowerService.Host.exe INSTALL
2. Two Appsettings in AppConfig RunIntervalInMinutes and ReportPath can be configured for schedule time interval and reports directory
3. Used log4Net for logging. Solution provides basic logging to debug issues and currently there is no alert raised if report fails.
4. Uses castle windsor for DI. Solution only has a basic implementation
5. Unit tests\Integration tests are very basic in interest of time but can be extended for edge and negative tests and more robust set.
6. Used a Query\QueryHandler pattern to isolate querying logic. Main reason behind it being that application can be extended to cater for new reports and provide separation of concerns (querying vs generation). Project Folder -> Queries\GetTrades
7. CsvWriter class takes a datatable and writes that datatable to a csv file. Reason for using datatable is reusability
8. AggregatedTradeReportGenerator brings the query and csvwriter together
9. There could be some unit tests for GetAggregatedTradesResult.ToDataTable
