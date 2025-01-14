// JS cho phần drop down để SORT (product)
var j = jQuery.noConflict();

j(".select-selected").click(function (e) {
    e.stopPropagation();
    j(this).css("border-color", "#000");
    j(this).next(".select-options").slideToggle("normal");
});

// cập nhật text mà ng dùng chọn
j(".select-option").click(function () {
    const selectedText = j(this).text();
    j(this)
        .closest(".custom-select")
        .find(".select-selected")
        .css("border-color", "#ccc");
    j(this).closest(".custom-select").find(".select-selected").text(selectedText);

    // đóng dropdown
    j(this).closest(".custom-select").find(".select-options").slideUp("1000");

});