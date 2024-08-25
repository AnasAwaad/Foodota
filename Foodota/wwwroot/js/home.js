
function toggleMenu() {
	$('.navigation').toggleClass('active');
}

$('.opening-hours').each(function () {
	if ($(this).text().trim() == '') {
		$(this).html(`<div>
						<i class="bi bi-clock-fill" style="color:green;"></i>
						<span class="text-danger">Closed</span>
					  </div>`)
	}
})

