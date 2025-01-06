//// responsive cho header ////

var j = jQuery.noConflict();

j("#open-sub-menu").on("click", function () {
  j("#header-dashboard").removeClass("open");
  j(this).hide();
  j("#main-content").css("padding-left", "300px");
});

j("#close-sub-menu").on("click", function () {
  j("#header-dashboard").addClass("open");
  j("#open-sub-menu").show();
  j("#main-content").css("padding-left", "20px");
});

//// SUB MENU ////

// Ngăn sự kiện nổi bọt khi click vào các phần tử bên trong .sub-menu
j(".sub-menu").on("click", function (event) {
  event.stopPropagation();
});
// mở đóng sub menu cho các lựa chọn
j(".item-dropdown").on("click", function () {
  j(this).find(".sub-menu").toggle();
  j(this).toggleClass("rotate");
});

j(".sub-menu-item a").on("click", function () {
  // Loại bỏ CSS màu sắc trước đó khỏi tất cả các thẻ <a>
  j(".sub-menu-item a").css("color", "");

  // Áp dụng CSS màu sắc chỉ cho thẻ <a> được click
  j(this).css("color", "#0a58ca");
});

