$(function () {
	var totalPrice = $('#totalPrice span');

	$('table').on('click', '.js-increase-amount', function () {
		var btn = $(this);
		$.ajax({
			url: "ShoppingCart/IncreaseItem/" + $(this).data('id'),
			success: function (res) {

				var itemPrice = btn.parents('tr').find('.js-item-price span').text();

				totalPrice.text(+totalPrice.text() + +itemPrice);

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
				var itemPrice = btn.parents('tr').find('.js-item-price span').text();

				totalPrice.text(+totalPrice.text() - +itemPrice);

				btn.parents('tr').replaceWith(res);
			}
		})
	})

	$('.js-remove-item').on('click', function () {
		var btn = $(this);

		$(this).parents('tr').fadeOut();
		setTimeout(() => {
			btn.parents('tr').remove();
		}, 500)

		$.ajax({
			url: "ShoppingCart/RemoveItem/" + $(this).data('id'),
			success: function (res) {
				var itemPrice = btn.parents('tr').find('.js-item-price span').text();
				var itemCount = btn.parents('tr').find('.js-count').text();
				console.log(itemCount)
				totalPrice.text(+totalPrice.text() - (+itemPrice * +itemCount));
				console.log("removed successfully");
			}
		})
	})
});