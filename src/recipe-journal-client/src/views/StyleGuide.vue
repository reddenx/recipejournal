<template>
    <div class="article-container">
        style guide Panels:
        <div class="panel">
            <div class="section">
                <div class="title">Recipe Title</div>
                <div>
                    <div>
                        <strong>Author:</strong> AuthorName
                        <strong>Time:</strong> 12345 active, 12345 total
                        <strong>Servings:</strong> 1234 PortionType
                        <button type="button">|||</button>
                    </div>
                </div>
                <p>Main Recipe Description Paragraph</p>
            </div>
            <div class="section dark">
                <div class="title">Ingredients</div>
            </div>
            <div class="section">
                <div class="ingredient">Garlic 2 cloves</div>
            </div>
            <div class="section dark">
                <div class="title">Component Name(optional)</div>
            </div>
            <div class="section">
                <div class="title">Step Title(optional)</div>
                <div>
                    <div class="ingredient">Garlic 2 cloves</div>
                </div>
                <p>description of step here</p>
            </div>
            <div class="section">
                <div></div>
                <p>description of component and step without titles</p>
            </div>
        </div>
        <br />
        <br />
        <br />

        style guide cms
        <div class="panel">
            <div class="section">
                <button type="type" class="cms-up"></button>
                <br />
                <button type="type" class="cms-down"></button>
            </div>
        </div>
        <br />
        <br />
        <br />

        style guide for forms
        <div class="panel">
            <div class="section">
                <p>blah blah blah do things</p>
                <button class="dark" type="button">confirm</button>
                <button type="button">cancel</button>
            </div>
        </div>
    </div>
</template>

<script>
export default {};
</script>

<style scoped>
/* page */
.article-container {
    max-width: 40em;
    margin-left: auto;
    margin-right: auto;
    padding: 0.25em;
}

.dark {
    background-color: rgb(53, 19, 0);
    color: white;
}

/* panels */
.panel {
    border: 4px solid rgb(53, 19, 0);
    border-radius: 1em;
}

.panel .section {
    box-sizing: border-box;
    border: none;
    padding: 0.6em 1.5em;
}

.panel .section p {
    text-align: justify;
    margin: 0;
    margin-top: 0.6em;
    line-height: 1.25;
}
.panel .section:last-child {
    margin-bottom: 0.8em;
}

.section .title {
    margin: 0;
    font-weight: bold;
    font-size: 1.6em;
}
.panel .section.dark .title {
    font-weight: 500;
}
.panel .section .title + div {
    margin-top: 0.6em;
}

/* inputs and controls */
button {
    border: 2px solid rgb(53, 19, 0);
    border-radius: 8em;
    min-height: 2.3em;
    padding: 0.2em 1.2em;
    box-sizing: border-box;
    font-weight: 500;
}
button:hover {
    background-color: gray;
    color: white;
}
button.dark {
    font-weight: 500;
}

button.cms-up {
    padding: 0;
    padding-top: 0.2em;
    font-size: 0.8em;
    width: 2.5em;
    max-width: 2.8em;
    min-width: 2.8em;
    height: 2.8em;
    min-height: 2.8em;
    max-height: 2.8em;
    border-radius: 0.8em;
    box-sizing: border-box;
}
button.cms-up::after {
    font-size: 1.5em;
    content: "\2BC5";
}
button.cms-down {
    font-size: 0.8em;
    padding: 0;
    width: 2.8em;
    max-width: 2.8em;
    min-width: 2.8em;
    height: 2.8em;
    min-height: 2.8em;
    max-height: 2.8em;
    border-radius: 0.8em;
    box-sizing: border-box;
}
button.cms-down:after {
    font-size: 1.5em;
    content: "\2BC6";
}
button.cms-edit {
}
button.cms-cancel {
}
button.cms-delete {
}
button.cms-save {
}

input[type="checkbox"] {
}
input[type="text"] {
}
textarea {
}

/* recipe specific things */
.ingredient::before {
    font-weight: 800;
    margin-right: 8px;
}
</style>