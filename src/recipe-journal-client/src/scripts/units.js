export default class Units {
    static getUnitForQty(unit, amount) {
        switch (unit) {
            case 'teaspoon':
                return amount == 1 ? ' tsp' : ' tsps';
            case 'tablespoon':
                return amount == 1 ? ' Tbsp' : ' Tbsps';
            case 'cup':
                return amount == 1 ? ' cup' : ' cups';
            case 'gram':
                return 'g';
        }
        return '';
    }
}