-- tbd
drop database if exists recipe;
create database recipe;

drop user if exists 'recipe_user';
create user 'recipe_user' identified by 'it#b3^that-time_for@cookins';
grant all privileges on recipe.* to 'recipe_user';

use recipe;

drop table if exists ingredient;
drop table if exists step_ingredient;
drop table if exists recipe_step;
drop table if exists recipe_component;
drop table if exists recipe;
drop table if exists account;


create table account
(
    Id varchar(32) primary key not null
    ,Username varchar(60) not null
);

create table recipe
(
    Id varchar(32) not null
    ,AccountId varchar(32) not null
    ,`Version` int not null
    ,DateCreated datetime not null
    ,VersionDate datetime not null
    ,Title varchar(800) not null
    ,`Description` text null
    ,DurationMinutes int null
    ,Servings int null
);

create table recipe_component
(
    Id varchar(32) primary key not null
    ,RecipeId varchar(32) not null
    ,Title varchar(800) null
    ,`Description` text null
);

create table recipe_step
(
    Id varchar(32) primary key not null
    ,ComponentId varchar(32) not null
    ,Title varchar(800) null
    ,Body text null
);

create table step_ingredient
(
    Id varchar(32) not null
    ,StepId varchar(32) not null
    ,IngredientId varchar(32) not null
    ,Unit varchar(20) not null
    ,Amount int not null
    ,`Description` int not null
);

create table ingredient
(
    Id varchar(32) not null
    ,`Name` varchar(800) not null
    ,`Description` text null
);

create table ingredient_category
(
    Id varchar(32) not null
    ,`Name` varchar(800) not null
);

create table ingredient_ingredient_category
(
    CategoryId varchar(32) not null
    ,IngredientId varchar(32) not null
);

insert into `account` (id, username) values ('00000000000000000000000000000001', 'sean');