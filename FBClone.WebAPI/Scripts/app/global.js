var MINOVATE = MINOVATE || {};

$(function() {

  //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
  // global inicialization functions
  //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

  MINOVATE.global = {

    init: function() {
      MINOVATE.global.deviceSize();
      MINOVATE.global.goToTop();
      MINOVATE.global.animsition();
    },

    // device identification function
    deviceSize: function(){
			var jRes = jRespond([
				{
					label: 'smallest',
					enter: 0,
					exit: 479
				},{
					label: 'handheld',
					enter: 480,
					exit: 767
				},{
					label: 'tablet',
					enter: 768,
					exit: 991
				},{
					label: 'laptop',
					enter: 992,
					exit: 1199
				},{
					label: 'desktop',
					enter: 1200,
					exit: 10000
				}
			]);
			jRes.addFunc([
				{
					breakpoint: 'desktop',
					enter: function() { $body.addClass('device-lg'); },
					exit: function() { $body.removeClass('device-lg'); }
				},{
					breakpoint: 'laptop',
					enter: function() { $body.addClass('device-md'); },
					exit: function() { $body.removeClass('device-md'); }
				},{
					breakpoint: 'tablet',
					enter: function() { $body.addClass('device-sm'); },
					exit: function() { $body.removeClass('device-sm'); }
				},{
					breakpoint: 'handheld',
					enter: function() { $body.addClass('device-xs'); },
					exit: function() { $body.removeClass('device-xs'); }
				},{
					breakpoint: 'smallest',
					enter: function() { $body.addClass('device-xxs'); },
					exit: function() { $body.removeClass('device-xxs'); }
				}
			]);
		},

    // scroll to top
    goToTop: function(){
			$goToTopEl.click(function() {
				$('body,html').stop(true).animate({scrollTop:0},400);
				return false;
			});
		},

	// show gotoTop element
    goToTopScroll: function(){
			if( $body.hasClass('device-lg') || $body.hasClass('device-md') || $body.hasClass('device-sm') ) {
				if($window.scrollTop() > 450) {
					$goToTopEl.fadeIn();
				} else {
					$goToTopEl.fadeOut();
				}
			}
		},

    // initialize animsition

    animsition: function() {
      $wrapper.animsition({
        inClass               :   'fade-in',
        outClass              :   'fade-out',
        inDuration            :    1500,
        outDuration           :    800,
        linkElement           :   '.animsition-link',
        // e.g. linkElement   :   'a:not([target="_blank"]):not([href^=#])'
        loading               :    true,
        loadingParentElement  :   'body', //animsition wrapper element
        loadingClass          :   'animsition-loading',
        unSupportCss          : [ 'animation-duration',
          '-webkit-animation-duration',
          '-o-animation-duration'
        ],
        //"unSupportCss" option allows you to disable the "animsition" in case the css property in the array is not supported by your browser.
        //The default setting is to disable the "animsition" in a browser that does not support "animation-duration".

        overlay               :   false,
        overlayClass          :   'animsition-overlay-slide',
        overlayParentElement  :   'body'
      });
    }

  };






  //!!!!!!!!!!!!!!!!!!!!!!!!!
  // header section functions
  //!!!!!!!!!!!!!!!!!!!!!!!!!

  MINOVATE.header = {

    init: function() {
      //MINOVATE.header.superfish();
      MINOVATE.header.searchToggle();
      MINOVATE.header.chooseMenuStyle();
      MINOVATE.header.chooseLogo();
      MINOVATE.header.stickyHeader();
      MINOVATE.header.menuClasses();
      //MINOVATE.header.toggleMenu();
    },


    //initialize superfish on main-navbar
    superfish: function(){

      if( $body.hasClass('device-lg') || $body.hasClass('device-md') ) {
        $('#main-navbar ul ul, #main-navbar ul .mega-menu-content').css('display', 'block');
			  MINOVATE.header.dropdownPosition();
      }

			$('#main-navbar > ul, #main-navbar > div > ul').superfish({
        popUpSelector: 'ul,.mega-menu-content',
        delay: 250,
        speed: 350,
        animation: {opacity:'show'},
        animationOut:  {opacity:'hide'},
        cssArrows: false
      });

		},

    //choose side for dropdown menu open
    dropdownPosition: function() {

			$('#main-navbar .mega-menu-content, #main-navbar ul ul').each( function( index, element ){
				var $dropdown = $(element);
				var windowWidth = $window.width();
				var dropdownOffset = $dropdown.offset();
				var dropdownWidth = $dropdown.width();
				var dropdownPosition = dropdownOffset.left;

				if(windowWidth - (dropdownWidth + dropdownPosition) < 0) {
					$dropdown.addClass('menu-right-position');
				}
			});

		},

    //toggle search box
    searchToggle: function(){

      $('#search-toggle a').click(function(e){
        e.preventDefault();

        if($searchBoxWrapper.height() == "0"){

          $('html, body').animate({
            scrollTop: 0
          }, 300, false);

          $('.search-box-wrapper input[type="search"]').focus();

          $searchBoxWrapper.animate({
            height:"50px"
          }, 300);

        } else {

          $searchBoxWrapper.animate({
            height:"0"
          }, 300);

        }

      });

    },

    // choose if logo need to be light or dark
    chooseLogo: function(){
			if($header.hasClass('light')) {
				if( defaultLightLogo ){ defaultLogo.find('img').attr('src', defaultLightLogo); }
				if( retinaLightLogo ){ retinaLogo.find('img').attr('src', retinaLightLogo); }
			} else {
				if( defaultLogoImg ){ defaultLogo.find('img').attr('src', defaultLogoImg); }
				if( retinaLogoImg ){ retinaLogo.find('img').attr('src', retinaLogoImg); }
			}
		},

    // makes header sticky
    stickyHeader: function( headerOffset ){
		if ($window.scrollTop() > headerOffset) {
			if( ( $body.hasClass('device-lg') || $body.hasClass('device-md') ) && !MINOVATE.isMobile.any() ) {
				$header.addClass('sticky-header');
				$header.removeClass('light');
			} else if( $body.hasClass('device-xs') || $body.hasClass('device-xxs') || $body.hasClass('device-sm') ) {
        if( $header.hasClass('sticky-mobile') ) { $header.addClass('responsive-sticky-header'); }
			} else {
				MINOVATE.header.unStickyHeader();
			}
		} else {
			MINOVATE.header.unStickyHeader();
		}
	},

    // remove sticky class on header
    unStickyHeader: function(){
		if( $header.hasClass('sticky-header') ){
			$header.removeClass('sticky-header');
			$header.removeClass().addClass(oldHeaderClasses);
		}
		if( $header.hasClass('responsive-sticky-header') ){
			$header.removeClass('responsive-sticky-header');
		}
	},

    // class toggling on menu according to windows width
    chooseMenuStyle: function(headerOffset) {
      if( $body.hasClass('device-xs') || $body.hasClass('device-xxs') || $body.hasClass('device-sm')) {
        $header.removeClass('light');
        if( !$header.hasClass('sticky-mobile') ) { MINOVATE.header.unStickyHeader(); }
      } else {
        if( $window.scrollTop() > headerOffset ) { $header.addClass('sticky-header'); }
        if( !$header.hasClass('sticky-header')) { $header.addClass('light'); }
        if( $header.hasClass('dark')) { $header.removeClass('light'); }
      }
    },

    // add sub-menu class to determined menu items
    menuClasses: function() {
      $( '#main-navbar ul li:has(ul)' ).addClass('submenu');

      if( MINOVATE.isMobile.Android() ) {
			$( '#main-navbar ul li.sub-menu' ).children('a').on('touchstart', function(e){
				if( !$(this).parent('li.submenu').hasClass('sfHover') ) {
					e.preventDefault();
				}
			});
		}
    },

    // toggling display of menu
    toggleMenu: function() {
        $menuToggler.click(function() {
			$( '#main-navbar > ul' ).toggleClass("show");
			return false;
		});
    }


  };
  
  



  //!!!!!!!!!!!!!!!!
  // extra functions
  //!!!!!!!!!!!!!!!!

  MINOVATE.extra = {

    init: function() {
      MINOVATE.extra.animations();
      //MINOVATE.extra.parallax();
      //MINOVATE.extra.loadFlexSlider();
      //MINOVATE.extra.lightbox();
      //MINOVATE.extra.flickrFeed();
      //MINOVATE.extra.twitterFeed();
      //MINOVATE.extra.html5Video();
      MINOVATE.extra.counter();
      //MINOVATE.extra.progress();
      //MINOVATE.extra.mixitup(); <-- This needs be initialized later than everything else due to login issues
    },


    //initialize animations on elements
    animations: function(){

      var $animateEl = $('[data-animate]');

      if( $animateEl.length > 0 ){

        if( $body.hasClass('device-lg') || $body.hasClass('device-md') || $body.hasClass('device-sm') ){

          $animateEl.each(function(){
            var el = $(this),
							  delay = el.attr('data-delay'),
							  delayTime = 0;

						if( delay ) { delayTime = Number( delay ) + 500; } else { delayTime = 500; }

						if( !el.hasClass('animated') ) {
							el.addClass('not-animated');
							var elAnimation = el.attr('data-animate');
							el.appear(function () {
								setTimeout(function() {
									el.removeClass('not-animated').addClass( elAnimation + ' animated');
								}, delayTime);
							},{accX: 0, accY: -120},'easeInCubic');
						}

					});

				}

			}

		},

    ////initialize magnificPopup lightbox
    //lightbox: function(){
	//		var $lightboxImageEl = $('[data-lightbox="image"]'),
    //      $lightboxIframeEl = $('[data-lightbox="iframe"]'),
    //      $lightboxGalleryEl = $('[data-lightbox="gallery"]');

	//		if( $lightboxImageEl.length > 0 ) {
	//			$lightboxImageEl.magnificPopup({
	//				type: 'image',
	//				closeOnContentClick: true,
	//				closeBtnInside: false,
	//				fixedContentPos: true,
	//				image: {
	//					verticalFit: true
	//				}
	//			});
	//		}

    //  if( $lightboxIframeEl.length > 0 ) {
	//			$lightboxIframeEl.magnificPopup({
	//				disableOn: 600,
	//				type: 'iframe',
	//				removalDelay: 160,
	//				preloader: false,
	//				fixedContentPos: false
	//			});
	//		}

	//		if( $lightboxGalleryEl.length > 0 ) {
	//			$lightboxGalleryEl.each(function() {
	//				var element = $(this);

	//				if( element.find('a[data-lightbox="gallery-item"]').parent('.clone').hasClass('clone') ) {
	//					element.find('a[data-lightbox="gallery-item"]').parent('.clone').find('a[data-lightbox="gallery-item"]').attr('data-lightbox','');
	//				}

	//				element.magnificPopup({
	//					delegate: 'a[data-lightbox="gallery-item"]',
	//					type: 'image',
	//					closeOnContentClick: true,
	//					closeBtnInside: false,
	//					fixedContentPos: true,
	//					image: {
	//						verticalFit: true
	//					},
	//					gallery: {
	//						enabled: true,
	//						navigateByImgClick: true,
	//						preload: [0,1] // Will preload 0 - before current, and 1 after the current image
	//					}
	//				});
	//			});
	//		}
	//	},

    //initialize countTo
    counter: function(){
      var $counterEl = $('.counter:not(.counter-instant)');
			if( $counterEl.length > 0 ){
				$counterEl.each(function(){
					var element = $(this);
          element.appear( function(){
            MINOVATE.extra.runCounter(element);
          });
				});
			}
    },

    //run countTo
    runCounter: function( counterElement){
		counterElement.find('span').countTo();
	},

    //animate progress bar
    progress: function(){
		var $progressEl = $('.progress');
		if( $progressEl.length > 0 ){
			$progressEl.each(function(){
				var progressBar = $(this),
						progressValue = progressBar.attr('data-percent');

				if( $body.hasClass('device-lg') || $body.hasClass('device-md') ){
					progressBar.appear( function(){
						if (!progressBar.hasClass('progress-animated')) {
							progressBar.find('.counter-instant span').countTo();
							progressBar.find('.progress-bar').css({width: progressValue + "%"}).addClass('progress-animated');
						}
					},{accX: 0, accY: -120},'easeInCubic');
				} else {
					progressBar.find('.counter-instant span').countTo();
					progressBar.find('.progress').css({width: progressValue + "%"});
				}
			});
		}
	},

    //initialize mixitup

    mixitup: function () {
		if( $mixitupEl.length > 0 ) {
			$mixitupEl.each(function() {
				var element = $(this);

				element.mixItUp({
				    callbacks: {
				        onMixFail: function (state) {
				            alert("no match " + state.activeFilter);
				        }
				    }
				});
			});
		}
	}
  };




  //!!!!!!!!!!!!!!!!!!!!
  // check mobile device
  //!!!!!!!!!!!!!!!!!!!!

  MINOVATE.isMobile = {
    Android: function() {
      return navigator.userAgent.match(/Android/i);
    },
    BlackBerry: function() {
      return navigator.userAgent.match(/BlackBerry/i);
    },
    iOS: function() {
      return navigator.userAgent.match(/iPhone|iPad|iPod/i);
    },
    Opera: function() {
      return navigator.userAgent.match(/Opera Mini/i);
    },
    Windows: function() {
      return navigator.userAgent.match(/IEMobile/i);
    },
    any: function() {
      return (MINOVATE.isMobile.Android() || MINOVATE.isMobile.BlackBerry() || MINOVATE.isMobile.iOS() || MINOVATE.isMobile.Opera() || MINOVATE.isMobile.Windows());
    }
  };



  //!!!!!!!!!!!!!!!!!!!!!!!!!
  // initialize after resize
  //!!!!!!!!!!!!!!!!!!!!!!!!!

  MINOVATE.documentOnResize = {

	  init: function(){

      var headerWrapOffset = $headerWrap.offset().top;

      var t = setTimeout( function(){
            MINOVATE.header.chooseMenuStyle(headerWrapOffset);
            MINOVATE.header.chooseLogo();
            //MINOVATE.extra.html5Video();
			}, 500 );

		}

	};






  //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
  // initialize when document ready
  //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

  MINOVATE.documentOnReady = {

      init: function () {
          MINOVATE.global.init();
          MINOVATE.header.init();
          MINOVATE.documentOnReady.windowscroll();
          MINOVATE.extra.init();
      },

      // run on window scrolling

      windowscroll: function () {

          var headerOffset = $header.offset().top;
          var headerWrapOffset = $headerWrap.offset().top;

          $window.on('scroll', function () {

              MINOVATE.global.goToTopScroll();
              MINOVATE.header.stickyHeader(headerWrapOffset);
              MINOVATE.header.chooseLogo();

          });
      }

  };







  //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
  // initialize when document load
  //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

	MINOVATE.documentOnLoad = {

		init: function(){

		}

	};






  //!!!!!!!!!!!!!!!!!!!!!!!!!
  // global variables
  //!!!!!!!!!!!!!!!!!!!!!!!!!

  var $window = $(window),
      $body = $('body'),
      $searchBoxWrapper = $('.search-box-wrapper'),
      $wrapper = $('#wrapper'),
      $header = $('#header'),
      $headerWrap = $('#header-wrap'),
      oldHeaderClasses = $header.attr('class'),
      defaultLogo = $('#branding').find('.brand-normal'),
      defaultLogoWidth = defaultLogo.find('img').outerWidth(),
      retinaLogo = $('#branding').find('.brand-retina'),
      defaultLogoImg = defaultLogo.find('img').attr('src'),
      retinaLogoImg = retinaLogo.find('img').attr('src'),
      defaultLightLogo = defaultLogo.attr('data-light-logo'),
      retinaLightLogo = retinaLogo.attr('data-light-logo'),
      $pageTitle = $('#page-title'),
      $slider = $('#slider'),
	  //$sliderParallaxEl = $('.slider-parallax'),
      //$menuToggler = $('#main-navbar-toggle'),
      //$owlCarouselEl = $('.owl-carousel'),
      $goToTopEl = $('#gotoTop'),
      $mixitupEl = $('.mix-grid');






  //!!!!!!!!!!!!!
  // initializing
  //!!!!!!!!!!!!!
  $(document).ready( MINOVATE.documentOnReady.init );
  $window.load( MINOVATE.documentOnLoad.init );
  $window.on( 'resize', MINOVATE.documentOnResize.init );

});
