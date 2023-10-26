$('#CartAdd').click(function () {
	$.ajax({
		url: "/Product/AddToCart",
		datatype: "text",
		type: "POST",
		success: function (data) {
			$alert(data);
		},
		error: function () {
			$alert("You Failed");
		}
	});
});