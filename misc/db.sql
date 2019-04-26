--Login with the AAD Admin

CREATE TABLE AzureGroups
(
 [Id] INT IDENTITY,
 [GroupName] NVARCHAR(128),
 [MeetupUrl] NVARCHAR(256)
);

INSERT INTO AzureGroups ([GroupName],[MeetupUrl]) VALUES ('Boston Azure', 'https://www.meetup.com/bostonazure/');
INSERT INTO AzureGroups ([GroupName],[MeetupUrl]) VALUES ('North Boston Azure', 'https://www.meetup.com/North-Boston-Azure-Cloud-User-Group/');

-- gab2019hol-group is the name of the AAD security group
CREATE USER [gab2019hol-group] FROM EXTERNAL PROVIDER;
GRANT SELECT ON dbo.AzureGroups TO [gab2019hol-group];

-- Here is how you would add access to a user
--CREATE USER [firstname.lastname@yourtenant.onmicrosoft.com] FROM EXTERNAL PROVIDER;
--GRANT SELECT ON dbo.Test TO [firstname.lastname@yourtenant.onmicrosoft.com];