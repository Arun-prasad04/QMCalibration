/**
 * @file jquery.translate.js
 * @brief jQuery plugin to translate text in the client side.
 * @author Manuel Fernandes
 * @site
 * @version 0.9
 * @license MIT license <http://www.opensource.org/licenses/MIT>
 *
 * translate.js is a jQuery plugin to translate text in the client side.
 *
 */

(function ($) {
    $.fn.translate = function (options) {
        var that = this; //a reference to ourselves

        var settings = {
            css: "trn",
            lang: "en"
        };

        settings = $.extend(settings, options || {});
        
        if (settings.css.lastIndexOf(".", 0) !== 0)   //doesn't start with '.'
            settings.css = "." + settings.css;

        var t = settings.t;
        
        //public methods
        this.lang = function (l) {
            if (l) {
                settings.lang = l;
                this.translate(settings);  //translate everything
            }

            return settings.lang;
        };


        this.get = function (index) {            
            var res = index;

            try {
                res = t[index][settings.lang];                
            }
            catch (err) {
                //not found, return index
                return index;
            }

            if (res)
                return res;
            else
                return index;
        };

        this.g = this.get;

        //main
        this.find(settings.css).each(function (i) {
            var $this = $(this);
            var trn_key = $this.attr("data-trn-key");
            var trn_Plc = $this.attr("placeholder");

            if (!trn_key) {

                if ($this.html() != "") {
                    trn_key = $this.html();
                }
                else {
                    trn_key = $this.val();
                }
                $this.attr("data-trn-key", trn_key);   //store key for next time
            }

            if (!trn_Plc) {               
                $this.attr('placeholder', trn_Plc);
            }


            if ($this.html() != "") {
                $this.html(that.get(trn_key));              
            }
            else {
                $this.val(that.get(trn_key));
                $this.attr("placeholder", that.get(trn_Plc));                
            }

        });

        return this;
    };
})(jQuery);