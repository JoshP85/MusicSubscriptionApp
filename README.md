# Music Subscription App

This project involved creating a light-weight music subscription app with c# and .net6 deployed to an AWS EC2 instance(Ubuntu Server 20.04/18.04 LTS AMI) using AWS services S3 and DynamoDB. It is a simple app that allows users to register, upload profile picture, edit account information, login, search for artists by artist name, song title or release year and add artists to their subscription list.   

Database tables are automatically created in DynamoDB and populated with seed data. Images are automatically downloaded from URLs specified in 'a2.json' and uploaded into S3.
