<template>
    <div class="article-container">


        <div class="panel">
            <div class="section">
                <div class="section-title">
                    Italian Macarons -- Some extra text here to see how this wraps
                </div>
                <div>
                    <strong>Author:</strong> sean <strong>Time:</strong> 3 hours <strong>Yields:</strong> 30 cookies 
                    <button type="button" class="slider-button"> ||| </button>
                </div>
                <p>
                    Lorem Ipson wrap test: ideally, the syrup will reach 118C around the same time the
                    meringue should be at the right state, this takes practice on
                    timing. The meringue is ready when it's still slightly foamy
                    but almost to soft peaks. remove syrup from heat and drizzle
                    slowly down the side of the mixing bowl while egg whites are
                    still being beaten, ensure the syrup is being picked up by the
                    beater and not landing in one area (trying to prevent the egg
                    from entirely cooking). beat this until stiff peaks, in the mean
                    time start your paste
                </p>
            </div>
            <div class="section-dark">
                some stuff could go here, probably buttons
            </div>
            <div class="section">
                <h3>Ingredients</h3>
                <div class="ingredient">egg white: 200g (100g/100g)</div>
                <div class="ingredient">sugar: 260g (230g/30g)</div>
                <div class="ingredient">water: 60g</div>
                <div class="ingredient">powdered sugar: 250g</div>
                <div class="ingredient">almond flour: 260g</div>
            </div>
            <div class="section-dark">
                <h3>Cookie</h3>
            </div>
            <div class="section">
                <p>
                    this section starts several things at once, it may be difficult to get the timing of the syrup
                    and the egg white foam, generally starting the eggs on low
                </p>
            </div>
            <div class="section">
                <h4>start eggs</h4>
                <div class="step-ingredients">
                    <div class="ingredient">egg white: 100g</div>
                    <div class="ingredient">sugar: 30g</div>
                </div>
                <p>
                    Place egg white in mixer and start whipping to foam, once
                    foaming add the stabilizer sugar
                </p>
            </div>
            <div class="section">
                <h4>start syrup</h4>
                <div class="step-ingredients">
                    <div class="ingredient">sugar 230g</div>
                    <div class="ingredient">water 60g</div>
                </div>
                <p>
                    Meanwhile mix sugar and water and put on heat. constantly stir 
                    once on heat
                </p>
            </div>
            <div class="section">
                <h4>start syrup</h4>
                <div class="step-ingredients">
                    <div class="ingredient">sugar 230g</div>
                    <div class="ingredient">water 60g</div>
                </div>
                <p>
                    Meanwhile mix sugar and water and put on heat. constantly stir once on heat
                </p>
            </div>
            <div class="section">
                <h4>combine</h4>
                <p>
                    ideally, the syrup will reach 118C around the same time the
                    meringue should be at the right state, this takes practice on
                    timing. The meringue is ready when it\'s still slightly foamy
                    but almost to soft peaks. remove syrup from heat and drizzle
                    slowly down the side of the mixing bowl while egg whites are
                    still being beaten, ensure the syrup is being picked up by the
                    beater and not landing in one area (trying to prevent the egg
                    from entirely cooking). beat this until stiff peaks, in the mean
                    time start your paste
                </p>
            </div>
            <div class="section">
                <h4>the paste</h4>
                <div class="step-ingredients">
                    <div class="ingredient">egg white 100g</div>
                    <div class="ingredient">powdered sugar 250g</div>
                    <div class="ingredient">almond flour 260g</div>
                </div>
                <p>
                    sift and combine egg white, powdered sugar, and almond floud, mix
                    gently trying not to add any air to the mixture
                </p>
            </div>
            <div class="section">
                <h4>fold</h4>
                <p>
                    once the meringue has reached stiff peaks, fold paste and
                    meringue until right consistency is reached, be sure not to be
                    too forceful and release the oils from the almond floud
                </p>
            </div>
            <div class="section">
                <h4>pipe and rest</h4>
                <p>
                    pipe onto silicon mat, let sit until skin is firm enough to bake 
                    (usually 10-30 min in semi-humid climate)
                </p>
            </div>
            <div class="section">
                <h4>bake</h4>
                <p>
                    bake at 300F for 10-17 minutes, experimentation still required 
                    here
                </p>
            </div>
            <div class="section-dark">
                <h3>Vanilla Buttercream</h3>
            </div>
            <div class="section">
                <div>
                    <div>Powdered Sugar: 2 cups</div>
                    <div>Butter: 12 Tbsp</div>
                    <div>Vanilla: 2 Tbsp</div>
                </div>
                <p>
                    Beat butter until it is creamy, slowly mix in powdered sugar until quite thick.
                    Add Vanilla.
                </p>
            </div>
            <div class="section-dark">
                <h3>Resting</h3>
            </div>
            <div class="section">
                <p>
                    This recipe generally requires at least 3-12 hours of rest with
                    the filling inside rehydrating the cookie.
                </p>
            </div>
        </div>
    </div>
</template>

<script>
export default {};
</script>

<style>
.article-container {
    max-width: 40rem;
    margin-left: auto;
    margin-right: auto;
}


.body {
    font-family: sans-serif;
}

button.slider-button {
    margin: 0;
    padding: 0;
    border: none;
    background: white;

    border: 2px solid rgb(173, 173, 173);
    border-radius: 4px;
    width:20px;
    height: 20px;
    font-size: 8px;
}

.ingredient::before {
    content: "\00BB";
    font-weight: 800;
    margin-right: 8px;
}

.panel {
    border: 4px solid rgb(53, 19, 0);
    border-radius: 10px;
    padding: 25px;
}
.panel p {
    text-align: justify;
    /* text-indent: 2rem; */
    margin: 0;
    margin-top: 10px;
    line-height: 1.25;
}
.section {
    margin-top:10px;
}
.section:nth-child(1) {
    margin-top:0;
}
.section-dark {
    margin-top:10px;
    background: rgb(53, 19, 0);
    color: white;
    box-sizing: border-box;
    border: none;
    margin-left: -25px;
    margin-right: -25px;
    padding: 10px 25px;
}
.panel h3 {
    margin:0;
}
.section-title {
    font-size: 32px;
    font-weight: 800;
    line-height: 1;
    margin-bottom: 15px;
}
.section ~ .section {
    padding-bottom: 10px;
}
.section + .section {
    border-top: 2px solid #f0f0f0;
    padding-top: 10px;
}

</style>