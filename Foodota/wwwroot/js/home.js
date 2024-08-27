
function toggleMenu() {
	$('.navigation').toggleClass('active');
}

/*toggle between hiding and showing the dropdown content */
function toggleDropDown() {
    document.getElementById("myDropdown").classList.toggle("show");
}

// Close the dropdown if the user clicks outside of it
window.onclick = function (event) {
    if (!event.target.matches('.dropbtn')) {
        var dropdowns = document.getElementsByClassName("dropdown-content");
        var i;
        for (i = 0; i < dropdowns.length; i++) {
            var openDropdown = dropdowns[i];
            if (openDropdown.classList.contains('show')) {
                openDropdown.classList.remove('show');
            }
        }
    }
}

$(function () {
    $('.js-open-cart').on('click', function () {
        console.log('click')
        $('body').addClass('showCart');
    });
    $('.js-close').on('click', function () {
        $('body').removeClass('showCart');
    });
    
})