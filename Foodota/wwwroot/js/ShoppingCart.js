﻿$(function () {
    const totalPrice = $('#totalPrice span');

    

    function updateTotalPrice(amount) {
        totalPrice.text((i, oldText) => +oldText + amount);
    }

    function loadCartCount() {
        $.ajax({
            url: "/ShoppingCart/GetTotalItemsInCart",
            success: function (res) {
                $('#cart-count').text(res.count);
            },
            error: function () {
                console.error("Failed to load cart count");
            }
        });
    }

    function loadShoppingCart() {
        $.ajax({
            url: "/ShoppingCart/GetAllItemsInCart",
            success: function (res) {
                $('.listCart').text('');
                res.cart.forEach(item => {
                    $('.listCart').append(`
                        <div class="item" id="item-${item.id}">
                            <div class="image">
                                <img src="${item.menuItem.imagePath}" class="w-100" />
                            </div>
                            <div class="name">${item.menuItem.name}</div>
                            <div class="totalPrice item-price-${item.id}">$ <span>${item.menuItem.mainPrice}</span></div>
                            <div class="quantity">
                                <span class="minus js-decrease-amount" data-id="${item.id}">-</span>
                                <span class="js-item-count">${item.count}</span>
                                <span class="plus js-increase-amount" data-id="${item.id}">+</span>
                            </div>
                        </div>
                    `);
                });
            },
            error: function () {
                console.error("Failed to load shopping cart");
            }
        });
    }

    // Load cart details if the user is logged in
    if ($('#username').text() !== '') {
        loadCartCount();
        loadShoppingCart();
    }

    // Event delegation for dynamically added elements
    $('body').on('click', '.js-add-to-cart', function () {
        if ($('#username').text() === '') {
            const returnUrl = window.location.pathname;
            window.location.href = "/identity/Account/Login?returnUrl=" + encodeURIComponent(returnUrl);
            return;
        }

        const menuItemId = $(this).data('itemid');
        const menuItemCount = $(this).parent().find('.order-count').val();

        $.ajax({
            url: "/ShoppingCart/AddToCart",
            type: "post",
            contentType: "application/json;charset=utf-8",
            data: JSON.stringify({ menuItemId, menuItemCount }),
            success: function (res) {
                loadShoppingCart();
                loadCartCount();

            },
            error: function () {
                console.error("Failed to add item to cart");
            }
        });
    });

    $('body').on('click', '.js-increase-amount', function () {
        const btn = $(this);
        const itemId = btn.data('id');
        $.ajax({
            url: `/ShoppingCart/IncreaseItem/${itemId}`,
            success: function (res) {
                
/*                IncreaseCountOfItem(itemId, 1);*/
                loadCartCount();
                loadShoppingCart();

                const itemPrice = +$('.item-price-' + itemId).find('span').text();
                updateTotalPrice(itemPrice);
                console.log(res)
                $('.item-row-'+itemId).replaceWith(res);
            },
            error: function () {
                console.error("Failed to increase item amount");
            }
        });
    });

    $('body').on('click', '.js-decrease-amount', function () {
        const btn = $(this);
        const itemId = btn.data('id');

        $.ajax({
            url: `/ShoppingCart/DecreaseItem/${itemId}`,
            success: function (res) {

                loadCartCount();
                loadShoppingCart();

                const itemCountElement = btn.next('.js-count');
                const currentCount = +itemCountElement.text();
                if (currentCount == 1) {
                    btn.parents('tr').remove();
                }

                itemCountElement.text(currentCount - 1);

                const itemPrice = +$('.item-price-' + itemId).find('span').text();
                updateTotalPrice(-itemPrice);

                $('item-row-'+itemId).replaceWith(res);
            },
            error: function () {
                console.error("Failed to decrease item amount");
            }
        });
    });

    $('body').on('click', '.js-remove-item', function () {
        const btn = $(this);
        const itemId = btn.data('id');


        $.ajax({
            url: `/ShoppingCart/RemoveItem/${itemId}`,
            success: function (res) {
                const itemPrice = +btn.parents('tr').find('.js-item-price span').text();
                const itemCount = +btn.parents('tr').find('.js-count').text();
                updateTotalPrice(-(itemPrice * itemCount));

                btn.parents('tr').fadeOut();
                setTimeout(() => btn.parents('tr').remove(), 500);
                $('#item-' + itemId).remove();
                $('#cart-count').text(+$('#cart-count').text() - 1);
                console.log("Item removed successfully");
            },
            error: function () {
                console.error("Failed to remove item from cart");
            }
        });
    });

    
});
