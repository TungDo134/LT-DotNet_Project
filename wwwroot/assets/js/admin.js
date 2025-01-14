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


// xác nhận xóa user
function confirmDelete() {
    if (!confirm("Bạn có chắc chắn muốn xóa" + '\n'
        + "Điều này sẽ xóa luôn tất cả đơn hàng của người dùng đó ")) {
        event.preventDefault(); // Hủy điều hướng
    }
}

// reset mật khẩu
function resetPassword(id) {
    j.ajax({

        url: 'https://localhost:7258/api/ResetAPI/reset-password/' + id,
        type: "POST",
        success: function (response) {
            alert(response.message); // Thông báo kết quả


        },
        error: function (xhr) {
            console.error("Error:", xhr.responseText);
            alert("Có lỗi xảy ra trong quá trình xóa sản phẩm.");
        }

    })
}

