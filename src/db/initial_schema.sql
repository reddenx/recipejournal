-- tbd
drop database if exists recipe;
create database recipe;

drop user if exists 'recipe_user';
create user 'recipe_user' identified by 'it#b3^that-time_for@cookins';
grant all privileges on recipe.* to 'recipe_user';

use recipe;

drop table if exists recipe_journal_entry;
drop table if exists shopping_recipe;
drop table if exists shopping_gathered;
drop table if exists ingredient_ingredient_category;
drop table if exists ingredient_category;
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
    ,PermissionsRole varchar(120) not null
    ,DateCreated datetime not null
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
    ,Published bit not null
    ,Public bit not null
);

create table recipe_component
(
    Id varchar(32) primary key not null
    ,RecipeId varchar(32) not null
    ,`Number` int not null
    ,Title varchar(800) null
    ,`Description` text null
);

create table recipe_step
(
    Id varchar(32) primary key not null
    ,`Number` int not null
    ,RecipeId varchar(32) not null
    ,ComponentId varchar(32) not null
    ,Title varchar(800) null
    ,Body text null
);

create table step_ingredient
(
    Id varchar(32) not null
    ,RecipeId varchar(32) not null
    ,StepId varchar(32) not null
    ,IngredientId varchar(32) not null
    ,`Number` int not null
    ,Unit varchar(20) not null
    ,Amount float not null
    ,`Description` text not null
);

create table ingredient
(
    Id varchar(32) not null
    ,`Name` varchar(800) not null
    ,`Description` text null
    ,DateCreated datetime not null
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

insert into `account` (id, username, permissionsrole, datecreated) values ('00000000000000000000000000000001', 'sean', 'admin', curdate());

create table shopping_recipe
(
    RecipeId varchar(32) not null,
    UserId varchar(32) not null,
    Scale float not null
);

create table shopping_gathered
(
    UserId varchar(32) not null,
    IngredientId varchar(32) not null
);

create table recipe_journal_entry
(
    Id varchar(32) not null,
    UserId varchar(32) not null,
    RecipeId varchar(32) not null,
    DateCreated datetime not null,
    DateModified datetime not null,

    EntryDate datetime not null,
    SuccessRating float not null,
    RecipeScale float not null,
    AttemptNotes text not null,
    ResultNotes text not null,
    GeneralNotes text not null,
    NextNotes text not null,
    StickyNext bit not null,
    NextDismissed bit not null
);