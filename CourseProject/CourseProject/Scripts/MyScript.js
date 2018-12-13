

var count = 0;

function addPosandIngr() {
    count++;
    var newId_pos = 'pos_list' + count.toString();
    var newId_ingr = 'ingr_list' + count.toString();

    var divchik = document.createElement('div');
    divchik.id = 'divchik' + count.toString();
    var first_div_pos = document.createElement('div');
    first_div_pos.className = 'form-group';
    var second_div_pos = document.createElement('div');
    second_div_pos.className = 'col-md-10';

    $("#pos_lbl").clone().appendTo(first_div_pos);
    $("#pos_list").clone().attr('id', newId_pos).appendTo(second_div_pos);
    $(second_div_pos).appendTo(first_div_pos);
    $(first_div_pos).appendTo(divchik);


    var first_div_ingr = document.createElement('div');
    first_div_ingr.className = 'form-group';
    var second_div_ingr = document.createElement('div');
    second_div_ingr.className = 'col-md-10';

    $("#ingr_lbl").clone().appendTo(first_div_ingr);
    $("#ingr_list").clone().attr('id', newId_ingr).appendTo(second_div_ingr);
    $(second_div_ingr).appendTo(first_div_ingr);
    $('<hr/>').appendTo(first_div_ingr);
    $(first_div_ingr).appendTo(divchik);

    $(divchik).appendTo('#container_for_add');
}


function delPosandIngr() {
    if (count != 0) {
        var divchik = document.getElementById('divchik' + count.toString());
        $(divchik).remove();
        count--;
    }
}

var counter_clck = 0;
function details(page_name) {
    var btn = document.getElementById('more');
    if (counter_clck % 2 == 0) {
        var positions = [];
        var ingredients = [];
        $("#pos_list option").each(function () {
            positions.push($(this).val());
        });

        $("#ingr_list option").each(function () {
            ingredients.push($(this).val());
        });

        var dt = document.createElement("dt");
        dt.innerHTML = 'Позиция|Ингредиент';
        dt.id = 'added_dt';
        $(dt).appendTo("#dtl");

        var dd = document.createElement("dd");
        dd.id = 'added_dd';
        var ul = document.createElement("ul");
        ul.id = 'added_ul';

        for (var i = 0; i < positions.length; i++) {
            var pos_li = document.createElement("li");
            pos_li.innerText = positions[i];
            if (page_name == 'Details') {
                pos_li.innerText += '|' + ingredients[i];
            }
            if (page_name == 'Edit') {
                $("#ingrs_list").clone().attr('id', 'ingrs_list' + i).val(ingredients[i]).appendTo(pos_li);
            }
            $(pos_li).appendTo(ul);
            $(ul).appendTo(dd);
            $(dd).appendTo("#dtl");
        }
        btn.value = 'Скрыть';
    }
    else {
        $("#added_dt").remove();
        $("#added_dd").remove();
        btn.value = 'Ещё...';
    }
    counter_clck++;
}