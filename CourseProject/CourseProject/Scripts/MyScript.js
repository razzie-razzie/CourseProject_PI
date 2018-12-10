
var count = 0;

function addPosandIngr() {
    count++;
    var newId_pos = 'pos_list' + count.toString();
    var newId_ingr = 'ingr_list' + count.toString();


    var first_div_pos = document.createElement('div');
    first_div_pos.className = 'form-group';
    var second_div_pos = document.createElement('div');
    second_div_pos.className = 'col-md-10';

    $("#pos_lbl").clone().appendTo(first_div_pos);
    $("#pos_list").clone().attr('id', newId_pos).appendTo(second_div_pos);
    $(second_div_pos).appendTo(first_div_pos);
    $(first_div_pos).appendTo('#container_for_add');


    var first_div_ingr = document.createElement('div');
    first_div_ingr.className = 'form-group';
    var second_div_ingr = document.createElement('div');
    second_div_ingr.className = 'col-md-10';

    $("#ingr_lbl").clone().appendTo(first_div_ingr);
    $("#ingr_list").clone().attr('id', newId_ingr).appendTo(second_div_ingr);
    $(second_div_ingr).appendTo(first_div_ingr);
    $(first_div_ingr).appendTo('#container_for_add');
    $('<hr style="color: black;"/>').appendTo('#container_for_add');

}

function GetPosandIngr() {
    for (i = 0; i <= count; i++) {
        if (i == 0) {
            var pos_val = document.getElementById('pos_list').value;
            var ingr_val = document.getElementById('ingr_list').value;
            alert(pos_val + ' ' + ingr_val);
            continue;
        }

        var pos_val = document.getElementById('pos_list' + count.toString()).value;
        var ingr_val = document.getElementById('ingr_list' + count.toString()).value;
        alert(pos_val + ' ' + ingr_val);
    }

}

