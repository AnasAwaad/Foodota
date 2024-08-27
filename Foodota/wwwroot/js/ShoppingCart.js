$(function () {
	$('table').on('click', '.js-increase-amount', function () {
		var btn = $(this);
		$.ajax({
			url: "ShoppingCart/IncreaseItem/" + $(this).data('id'),
			success: function (res) {
				console.log(res);
				btn.parents('tr').replaceWith(res);
			}
		})
	})

	$('table').on('click', '.js-decrease-amount', function () {
		var btn = $(this);
		if (btn.parent().find('.js-count').text() == 1) {
			btn.parents('tr').fadeOut();
			setTimeout(() => {
				btn.parents('tr').remove();

			}, 500)
		}
		$.ajax({
			url: "ShoppingCart/DecreaseItem/" + $(this).data('id'),
			success: function (res) {
				btn.parents('tr').replaceWith(res);
			}
		})
	})
})