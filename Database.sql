Create database EmployeeDbCore1
use EmployeeDbCore1

Create table Employee(
EmployeeId Int primary key identity,
EmploeeName varchar(100) not null,
Joindate date not null,
Image varchar(max),
ActiveStatus bit
)

Create table Skill(
SkillId Int Primary Key identity,
SkillName Varchar(50) Not null
)

Create table EmployeeSkill(
EmployeeSkillId int Primary key identity,
EmployeeId int references Employee(EmployeeId),
SkillId int references Skill(SkillId)

)

insert into Skill values ('SQL')
insert into Skill values ('C#')
insert into Skill values ('MVC')
insert into Skill values ('.NET Core')
Select * from Skill