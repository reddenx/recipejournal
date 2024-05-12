-- tbd
use recipe;

drop table if exists shopping_nonrecipe_ingredient;

create table shopping_nonrecipe_ingredient
(
    UserId varchar(32) not null,
    IngredientId varchar(32) null,
    Amount varchar(32) null,
)
