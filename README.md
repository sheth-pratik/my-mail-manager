# my-mail-manager
Sample project for viewing your Google Mail through SMTP


Note: This document contains steps to create Database, Steps to build source code and assumptions and missing requirements

1 Steps to create DB.
- Open src > dbscript > Script.Sql in SQL Server Management Studio (2014 preferred)
- Verify and Change below two files if required
	- MyWebMail (Line 8) - (Change FILENAME to path which exist)
	- MyWebMail_log (Line 10) - (Change FILENAME to path which exist)
- Execute Script
  (This will create DB named  MyWebMail with all necessary tables.)

2 Steps to build source code
- Open src > MyMailWeb > MyMailWeb.sln
- Open Web.config in MyMailWeb project
- Change following 2 connection string with server name, userid, password, dbname of server where script was executed.
	- DefaultConnection (Used for Authentication)
	- MyWebMailEntities (Used for Mail Configuration) 
- Build Project and Run it.
- ~~For "how to operate website" refer Wink file under Wink folder~~ (Not committed)

3 Missing requirements and known issues.
- If you have not configured IMAP, follow below link to allow it
https://support.google.com/mail/troubleshooter/1668960?hl=en#ts=1665018
- If not done Turn on "Access for less secure apps" from below link
https://www.google.com/settings/security/lesssecureapps

Known Issues:
- Currently IMAP Password and SMTP Password are stored in plain format in DB.
- Mail count displayed wil mail box and on Mail box title are sometimes not updated.
- Basic functionalities like Forward, Reply, Replay All are not implemented. (But disabled buttons are provided for future enhancement)
- It has been observed that connection with Imap Client is disconnected after some time.
  - Solution - Logout and log in again should solve issue.

4 Possible enhancement in App
- Implementing missing Forward, Reply, Reply all functionality
- Search facility
- Implement SignalR to get real time notification from server for add/edit/delete mails.
