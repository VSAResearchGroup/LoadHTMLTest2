select * from JobType;
insert into JobType values('Full Time',40,1);
insert into JobType values('Part Time',20,1);
insert into JobType values('Unemployed',0,1);

delete from JobType
where JobType.ID = 6;

select * from TimePreference;
insert into TimePreference(TimePeriod,Status) values('Morning',1);
insert into TimePreference(TimePeriod,Status) values('Afternoon',1);
insert into TimePreference(TimePeriod,Status) values('Evening',1);

delete from TimePreference
where ID >= 5;

CREATE table EnrollmentType(
	ID	int NOT NULL,
	EnrollmentDescription	varchar(30) NOT NULL,

	Primary key(ID)
);
select * from EnrollmentType;
insert into EnrollmentType values(1,'Full Time');
insert into EnrollmentType values(2,'Part Time');

select * from Quarter;

select * from major;

select * from School;
delete from School 
where ID = 5;
insert into School(Name,Acronymn,Address) values ('University of Washington Bothell','UWB','Bothell,WA');

select * from Course;

select Course.CourseNumber from Course
where Course.CourseID = 520;

select Course.CourseID from Course
where Course.CourseNumber = 'FRCH 122';

select * from PlanRatingDescription;
insert into PlanRatingDescription values('Too many electives');

select PlanRatingDescription.ID from PlanRatingDescription
where PlanRatingDescription.Description = 'too many electives';

select * from ParameterSet;
delete from ParameterSet
where ID = 2518;

Alter table ParameterSet 
ADD [SummerPreference] VARCHAR(10),
	 [EnrollmentType] VARCHAR (10);

insert into ParameterSet(MajorID,SchoolID,JobTypeID,TimePreferenceID,QuarterPreferenceID,DateAdded,LastDateModified,Status,SummerPreference,EnrollmentType)
select Major.ID,School.ID,JobType.ID,TimePreference.ID,Quarter.QuarterID,DateAdded = CURRENT_TIMESTAMP,LastDateModified = CURRENT_TIMESTAMP,Status=1,SummerPreference ='No',EnrollmentType.ID
from Major,School,JobType,TimePreference,Quarter,EnrollmentType
where
	Major.Name = 'Mechanical Engineering'
AND School.Acronymn = 'UW'
AND JobType.JobType = 'Part Time'
AND TimePreference.TimePeriod = 'Morning'
AND Quarter.Quarter = 'Fall'
AND EnrollmentType.EnrollmentDescription = 'Part Time';

select * from ParameterSet

select ParameterSet.ID from ParameterSet
where MajorID IN 
	(select Major.ID from Major
	where
	Major.Name = 'Mechanical Engineering')

AND SchoolID IN
	(select School.ID from School
	 where
	School.Acronymn = 'UW')
AND JobTypeID IN
	(select JobType.ID from JobType
	where
	JobType.JobType = 'Part Time'
	)
AND TimePreferenceID IN
	(select TimePreference.ID from TimePreference
	where
	TimePreference.TimePeriod = 'Morning')
AND QuarterPreferenceID IN
	(Select Quarter.QuarterID from Quarter
	where
	Quarter.Quarter = 'Fall')
AND SummerPreference = 'No'
AND EnrollmentType IN
	(select EnrollmentType.ID from EnrollmentType
	where
	EnrollmentType.EnrollmentDescription = 'Part Time');

CREATE TABLE [dbo].[GeneratedPlan] (
    [ID]               INT          IDENTITY (1, 1) NOT NULL,
    [Name]             VARCHAR (50) NULL,
    [Score]            FLOAT (53)   NULL,
    [ParameterSetID]   INT          NULL,
    [DateAdded]        DATETIME     NULL,
    [LastDateModified] DATETIME     NULL,
    [Status]           INT          NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
	CONSTRAINT [FK_GeneratedPlan_ParameterSet] FOREIGN KEY ([ParameterSetID]) REFERENCES [dbo].[ParameterSet] ([ID])
);

select * from GeneratedPlan;
drop table GeneratedPlan;
delete from GeneratedPlan
where ID >= 2837 AND ID <= 2859;

insert into GeneratedPlan(Name,ParameterSetID,DateAdded,LastDateModified,Status)
select Name = 'Latest',ParameterSet.ID,DateAdded = CURRENT_TIMESTAMP,LastDateModified = CURRENT_TIMESTAMP,Status=1
from ParameterSet
where MajorID IN 
	(select Major.ID from Major
	where
	Major.Name = 'Mechanical Engineering')

AND SchoolID IN
	(select School.ID from School
	 where
	School.Acronymn = 'UW')
AND JobTypeID IN
	(select JobType.ID from JobType
	where
	JobType.JobType = 'Part Time'
	)
AND TimePreferenceID IN
	(select TimePreference.ID from TimePreference
	where
	TimePreference.TimePeriod = 'Morning')
AND QuarterPreferenceID IN
	(Select Quarter.QuarterID from Quarter
	where
	Quarter.Quarter = 'Fall')
AND SummerPreference = 'No'
AND EnrollmentType IN
	(select EnrollmentType.ID from EnrollmentType
	where
	EnrollmentType.EnrollmentDescription = 'Part Time');

select * from StudyPlan;
drop table StudyPlan;
delete from StudyPlan
where ID >= 51788 AND ID <=53977;

CREATE TABLE [dbo].[StudyPlan] (
    [ID]               INT      IDENTITY (1, 1) NOT NULL,
    [PlanID]           INT      NULL,
    [QuarterID]        INT      NULL,
    [YearID]           INT      NULL,
    [CourseID]         INT      NULL,
    [DateAdded]        DATETIME NULL,
    [LastDateModified] DATETIME NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_StudentModifiedStudyPlan_GeneratedPlan] FOREIGN KEY ([PlanID]) REFERENCES [dbo].[GeneratedPlan] ([ID]),
    CONSTRAINT [FK_StudyPlan_GeneratedPlan] FOREIGN KEY ([PlanID]) REFERENCES [dbo].[GeneratedPlan] ([ID]),
	CONSTRAINT [FK_StudyPlan_Course] FOREIGN KEY ([CourseID]) REFERENCES [dbo].[course] ([ID]),
	CONSTRAINT [FK_StudyPlan_Quarter] FOREIGN KEY ([QuarterID]) REFERENCES [dbo].[Quarter] ([ID])
);

insert into StudyPlan(PlanID,QuarterID,YearID,CourseID,DateAdded,LastDateModified)
select GeneratedPlan.ID,Quarter.QuarterID,YearID = 2016,course.CourseID,DateAdded = CURRENT_TIMESTAMP, LastDateModified = CURRENT_TIMESTAMP
from GeneratedPlan,Quarter,course
where
	GeneratedPlan.ID IN
	(select GeneratedPlan.ID from GeneratedPlan
	where
	ParameterSetID IN
		(
			select ParameterSet.ID from ParameterSet
			where MajorID IN 
				(select Major.ID from Major
				where
				Major.Name = 'Mechanical Engineering')
			AND SchoolID IN
				(select School.ID from School
				where
				School.Acronymn = 'UW')
			AND JobTypeID IN
				(select JobType.ID from JobType
				where
				JobType.JobType = 'Part Time')
			AND TimePreferenceID IN
				(select TimePreference.ID from TimePreference
				where
				TimePreference.TimePeriod = 'Morning')
			AND QuarterPreferenceID IN
				(Select Quarter.QuarterID from Quarter
				where
				Quarter.Quarter = 'Fall')
			AND SummerPreference = 'No'
			AND EnrollmentType IN
			(select EnrollmentType.ID from EnrollmentType
			where
				EnrollmentType.EnrollmentDescription = 'Part Time')
		)
	)
AND	Quarter.QuarterID IN
	(select Quarter.QuarterID from Quarter
	where
	Quarter.Quarter = 'Winter')
AND course.CourseID IN
	(select course.CourseID from course
	where
	course.CourseNumber= 'CHEM& 110');

select * from StudentStudyPlan;
delete from StudentStudyPlan
where ID = 2802;

insert into StudentStudyPlan(StudentID,PlanID,CreationDate,LastDateModified,Status)
select StudentID = 777,GeneratedPlan.ID,creationDate = CURRENT_TIMESTAMP,LastDateModified = CURRENT_TIMESTAMP,Status = 1
from GeneratedPlan
where
	GeneratedPlan.ID IN
	(select GeneratedPlan.ID from GeneratedPlan
	where
	ParameterSetID IN
		(
			select ParameterSet.ID from ParameterSet
			where MajorID IN 
				(select Major.ID from Major
				where
				Major.Name = 'Mechanical Engineering')
			AND SchoolID IN
				(select School.ID from School
				where
				School.Acronymn = 'UW')
			AND JobTypeID IN
				(select JobType.ID from JobType
				where
				JobType.JobType = 'Part Time')
			AND TimePreferenceID IN
				(select TimePreference.ID from TimePreference
				where
				TimePreference.TimePeriod = 'Morning')
			AND QuarterPreferenceID IN
				(Select Quarter.QuarterID from Quarter
				where
				Quarter.Quarter = 'Fall')
			AND SummerPreference = 'No'
			AND EnrollmentType IN
			(select EnrollmentType.ID from EnrollmentType
			where
				EnrollmentType.EnrollmentDescription = 'Part Time')
		)
	);

select * from PlanRating;
delete from PlanRating;
select * from PlanRatingOption;
select * from PlanRatingDescription;
drop table planRating;

CREATE TABLE [dbo].[PlanRating] (
	[ID]            INT IDENTITY (1, 1) NOT NULL,
    [PlanID]        INT NOT NULL,
    [DescriptionID] INT NOT NULL,
    [Stars]         INT NULL,
    CONSTRAINT [PK_PlanRating] PRIMARY KEY CLUSTERED ([ID] ASC),
	CONSTRAINT [FK_PlanRating] Foreign key(DescriptionID) references planRatingDescription(ID)
);

insert into PlanRating(PlanID,DescriptionID,Stars)
select GeneratedPlan.ID,PlanRatingDescription.ID,Stars = 1
from GeneratedPlan,PlanRatingDescription
where
	GeneratedPlan.ID IN
	(select GeneratedPlan.ID from GeneratedPlan
	where
	ParameterSetID IN
		(
			select ParameterSet.ID from ParameterSet
			where MajorID IN 
				(select Major.ID from Major
				where
				Major.Name = 'Mechanical Engineering')
			AND SchoolID IN
				(select School.ID from School
				where
				School.Acronymn = 'UW')
			AND JobTypeID IN
				(select JobType.ID from JobType
				where
				JobType.JobType = 'Part Time')
			AND TimePreferenceID IN
				(select TimePreference.ID from TimePreference
				where
				TimePreference.TimePeriod = 'Morning')
			AND QuarterPreferenceID IN
				(Select Quarter.QuarterID from Quarter
				where
				Quarter.Quarter = 'Fall')
			AND SummerPreference = 'No'
			AND EnrollmentType IN
			(select EnrollmentType.ID from EnrollmentType
			where
				EnrollmentType.EnrollmentDescription = 'Part Time')
		)
	)
	AND PlanRatingDescription.ID IN
	(
		select ID from PlanRatingDescription
		where
			Description = 'Too long'
	);
select * from Major;
select * from ParameterSet;

insert into ParameterSet(MajorID,SchoolID,JobTypeID,TimePreferenceID,QuarterPreferenceID,DateAdded,LastDateModified,Status,SummerPreference,EnrollmentType)
select Major.ID,School.ID,JobType.ID,TimePreference.ID,Quarter.QuarterID,DateAdded = CURRENT_TIMESTAMP,LastDateModified = CURRENT_TIMESTAMP,Status=1,SummerPreference ='Yes',EnrollmentType.ID
from Major,School,JobType,TimePreference,Quarter,EnrollmentType
where
	Major.Name = 'Mechanical Engineering'
AND School.Acronymn = 'WSU'
AND JobType.JobType = 'Unemployed'
AND TimePreference.TimePeriod = 'Evening'
AND Quarter.Quarter = 'Fall'
AND EnrollmentType.EnrollmentDescription = 'Full Time';

delete from ParameterSet;

select ParameterSet.ID from ParameterSet
where MajorID IN 
	(select Major.ID from Major
	where
	Major.Name = 'Mechanical Engineering')

AND SchoolID IN
	(select School.ID from School
	 where
	School.Acronymn = 'WSU')
AND JobTypeID IN
	(select JobType.ID from JobType
	where
	JobType.JobType = 'Unemployed'
	)
AND TimePreferenceID IN
	(select TimePreference.ID from TimePreference
	where
	TimePreference.TimePeriod = 'Evening')
AND QuarterPreferenceID IN
	(Select Quarter.QuarterID from Quarter
	where
	Quarter.Quarter = 'Fall')
AND SummerPreference = 'Yes'
AND EnrollmentType IN
	(select EnrollmentType.ID from EnrollmentType
	where
	EnrollmentType.EnrollmentDescription = 'Full Time');

insert into GeneratedPlan(Name,ParameterSetID,DateAdded,LastDateModified,Status)
select Name = 'Latest',ParameterSet.ID,DateAdded = CURRENT_TIMESTAMP,LastDateModified = CURRENT_TIMESTAMP,Status=1
from ParameterSet
where MajorID IN 
	(select Major.ID from Major
	where
	Major.Name = 'Mechanical Engineering')

AND SchoolID IN
	(select School.ID from School
	 where
	School.Acronymn = 'WSU')
AND JobTypeID IN
	(select JobType.ID from JobType
	where
	JobType.JobType = 'Unemployed'
	)
AND TimePreferenceID IN
	(select TimePreference.ID from TimePreference
	where
	TimePreference.TimePeriod = 'Evening')
AND QuarterPreferenceID IN
	(Select Quarter.QuarterID from Quarter
	where
	Quarter.Quarter = 'Fall')
AND SummerPreference = 'Yes'
AND EnrollmentType IN
	(select EnrollmentType.ID from EnrollmentType
	where
	EnrollmentType.EnrollmentDescription = 'Full Time');
select * from GeneratedPlan;
delete from GeneratedPlan;

select GeneratedPlan.ID from GeneratedPlan
	where
	ParameterSetID IN
		(
			select ParameterSet.ID from ParameterSet
			where MajorID IN 
				(select Major.ID from Major
				where
				Major.Name = 'Mechanical Engineering')
			AND SchoolID IN
				(select School.ID from School
				where
				School.Acronymn = 'WSU')
			AND JobTypeID IN
				(select JobType.ID from JobType
				where
				JobType.JobType = 'Unemployed')
			AND TimePreferenceID IN
				(select TimePreference.ID from TimePreference
				where
				TimePreference.TimePeriod = 'Evening')
			AND QuarterPreferenceID IN
				(Select Quarter.QuarterID from Quarter
				where
				Quarter.Quarter = 'Fall')
			AND SummerPreference = 'Yes'
			AND EnrollmentType IN
			(select EnrollmentType.ID from EnrollmentType
			where
				EnrollmentType.EnrollmentDescription = 'Full Time')
		);
select * from PlanRating;
delete from PlanRating;
insert into PlanRating(PlanID,DescriptionID,Stars)
select GeneratedPlan.ID,PlanRatingDescription.ID,Stars = 1
from GeneratedPlan,PlanRatingDescription
where
	GeneratedPlan.ID IN
	(select GeneratedPlan.ID from GeneratedPlan
	where
	ParameterSetID IN
		(
			select ParameterSet.ID from ParameterSet
			where MajorID IN 
				(select Major.ID from Major
				where
				Major.Name = 'Mechanical Engineering')
			AND SchoolID IN
				(select School.ID from School
				where
				School.Acronymn = 'WSU')
			AND JobTypeID IN
				(select JobType.ID from JobType
				where
				JobType.JobType = 'Unemployed')
			AND TimePreferenceID IN
				(select TimePreference.ID from TimePreference
				where
				TimePreference.TimePeriod = 'Evening')
			AND QuarterPreferenceID IN
				(Select Quarter.QuarterID from Quarter
				where
				Quarter.Quarter = 'Fall')
			AND SummerPreference = 'Yes'
			AND EnrollmentType IN
			(select EnrollmentType.ID from EnrollmentType
			where
				EnrollmentType.EnrollmentDescription = 'Full Time')
		)
	)
	AND PlanRatingDescription.ID IN
	(
		select ID from PlanRatingDescription
		where
			Description = 'Too long'
	);

	