<template>
    <div class="article-container">
        <div class="article-info">
            <div class="article-title">Italian Macarons</div>
            <div class="article-description">
                a harder way to do the thing that was already hard!
            </div>
            3 hours 30 servings
        </div>

        <div class="article-ingredients">
            <div class="ingredient">egg white*: 200g</div>
            <div class="ingredient">sugar*: 260g</div>
            <div class="ingredient">water: 60g</div>
            <div class="ingredient">powdered sugar: 250g</div>
            <div class="ingredient">almond floud: 260g</div>
        </div>

        <div class="step">
            <div class="step-ingredients">
                <div class="ingredient">egg white 100g</div>
                <div class="ingredient">sugar 30g</div>
            </div>

            <div class="step-title">start eggs</div>
            <div class="step-body">
                Place egg white in mixer and start whipping to foam, once
                foaming add the stabilizer sugar
            </div>
        </div>

        <div class="step">
            <div class="step-ingredients">
                <div class="ingredient">sugar 230g</div>
                <div class="ingredient">water 60g</div>
            </div>
            <div class="step-title">start syrup</div>
            <div class="step-body">
                Meanwhile mix sugar and water and put on heat. constantly stir
                once on heat
            </div>
        </div>

        <div class="step">
            <div class="step-title">combine</div>
            <div class="step-body">
                ideally, the syrup will reach 118C around the same time the
                meringue should be at the right state, this takes practice on
                timing. The meringue is ready when it\'s still slightly foamy
                but almost to soft peaks. remove syrup from heat and drizzle
                slowly down the side of the mixing bowl while egg whites are
                still being beaten, ensure the syrup is being picked up by the
                beater and not landing in one area (trying to prevent the egg
                from entirely cooking). beat this until stiff peaks, in the mean
                time start your paste
            </div>
        </div>

        <div class="step">
            <div class="step-ingredients">
                <div class="ingredient">egg white 100g</div>
                <div class="ingredient">powdered sugar 250g</div>
                <div class="ingredient">almond flour 260g</div>
            </div>
            <div class="step-title">the paste</div>
            <div class="step-body">
                sift and combine egg white, powdered sugar, and almond floud,
                mix gently trying not to add any air to the mixture
            </div>
        </div>

        <div class="step">
            <div class="step-title">fold</div>
            <div class="step-body">
                once the meringue has reached stiff peaks, fold paste and
                meringue until right consistency is reached, be sure not to be
                too forceful and release the oils from the almond floud
            </div>
        </div>

        <div class="step">
            <div class="step-title">pipe and rest</div>
            <div class="step-body">
                pipe onto silicon mat, let sit until skin is firm enough to bake
                (usually 10-30 min in semi-humid climate)
            </div>
        </div>

        <div class="step">
            <div class="step-title">bake</div>
            <div class="step-body">
                bake at 300F for 10-17 minutes, experimentation still required
                here
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
    background: #ffe4e4;
}

.step {
    margin: 0 0 1em 0;
    background: #fffce4;
    padding: 0 1rem;
}

.step-title {
    font-size: 1.3rem;
    font-weight: bold;
    background: #e4ffec;
}

.step-ingredients {
    background: #e4fffb;
}
.step-body {
    text-indent: 2rem;
    text-align: justify;
    background: #e4e6ff;
}
</style>