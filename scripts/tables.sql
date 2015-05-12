
CREATE TABLE `categories` (
  `Id` varchar(60) NOT NULL,
  `Code` varchar(60) NOT NULL,
  `Name` varchar(60) NOT NULL,
  `Desc` varchar(512) DEFAULT NULL,
  `Sequence` bigint(20) NOT NULL DEFAULT '0',
  `Scope` int(11) NOT NULL,
  `ParentId` varchar(60) DEFAULT NULL,
  `Disused` tinyint(1) NOT NULL,
  `Creation` datetime NOT NULL,
  `Modification` datetime NOT NULL,
  PRIMARY KEY (`Id`)
);