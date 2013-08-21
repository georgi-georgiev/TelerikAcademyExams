var controls = (function(){
	"use stric";

	function ImageGallery(selector){
		var selector = document.querySelector(selector);

		this.addImage = function(name, path){
			var imageCont = document.createElement("div");
			imageCont.className = "imageCont";
			var title = document.createElement("p");
			name = name.replace(/\<br\>/g, "<\bbr>");
			title.innerHTML = name;
			imageCont.appendChild(title);
			var image = document.createElement("img");
			image.onclick = zoom;
			image.title = name;
			image.src = path;
			image.alt = "&nbsp;";
			imageCont.appendChild(image);
			selector.appendChild(imageCont);
		};

		this.addAlbum = function(title){
			title = title.replace(/\r\n|\n/, '<br />');
			var id = title.replace(/\s/g, "");//remove space from title to work correctly with id
			var albumCont = document.createElement("div");
			albumCont.className = "albumCont clearfix";
			albumCont.id = id;
			var albumTitle = document.createElement("p");
			albumTitle.className = "albumTitle";
			albumTitle.innerHTML = title;
			albumTitle.onclick = collapse;
			albumCont.appendChild(albumTitle);
			var sortButton = document.createElement("button");
			sortButton.innerHTML = "sort";
			sortButton.onclick = sortAlbum;
			albumCont.appendChild(sortButton);
			selector.appendChild(albumCont);
			return new ImageGallery("#"+id);
		};

		function collapse(e){
			var target = e ? e.target : event.srcElement;
		    if (!/option|select/i.test(target.nodeName)) {
		        var parrent = document.getElementById(target.parentNode.id);
		        var parrentChilds = parrent.childNodes;
		        for(var i=1, len = parrentChilds.length; i < len; i++){
		        	if(parrentChilds[i].style.display == "none"){
		        		parrentChilds[i].style.display = "block";
		        	}
		        	else{
		        		parrentChilds[i].style.display = "none";
		        	}
		        }
	    	}
		}

		function sortAlbum(e){
			var target = e ? e.target : event.srcElement;
		    if (!/option|select/i.test(target.nodeName)) {
				var parrent = document.getElementById(target.parentNode.id);
		        var titles = parrent.getElementsByClassName("albumTitle");
		        var titlesArray = new Array();
		        var i;
		        for(i=1, len = titles.length; i<len; i++){
		        	titlesArray[i] = titles[i].innerHTML;
		        }
		        titlesArray.sort();
		        alert(titlesArray);
			}
		}

		function zoom(e){
			var target = e ? e.target : event.srcElement;
		    if (!/option|select/i.test(target.nodeName)) {
		    	var zoomImageCont = document.getElementById("showImgZoomed");
		    	var zoomImage = zoomImageCont.childNodes[0];
	    		zoomImageCont.removeChild(zoomImage);
		    	var cloneImage = target.cloneNode(true);
		    	cloneImage.width = cloneImage.width*2;
		    	document.getElementById("showImgZoomed").appendChild(cloneImage);
			}
		}

		this.getImageGalleryData = function(){
			return this;
		};

	}

	function ImageGalleryRepository(){
		this.load = function(gallery){
			localStorage.getItem(gallery);
		};

		this.save = function(gallery, imageGalleryData){
			localStorage.setItem(gallery, imageGalleryData);
		};
	}

	function build(gallery, imageGalleryData) {
	    var gallery = gallery.addImage(imageGalleryData.title);
	    if (imageGalleryData) {
	  		for (var i = 0; i < imageGalleryData.length; i++) {
	        	build(gallery, imageGalleryData[i]);
	  		}
		}
  	}
	return {
		getImageGallery: function(selector){
			return new ImageGallery(selector);
		},
		buildImageGallery: function(gallery, imageGalleryData){
			var gallery = this.getImageGallery(gallery);

			if (imageGalleryData) {
				for (var i = 0; i < imageGalleryData.length; i++) {
			  		build(gallery, imageGalleryData[i]);
				}
			}

			return gallery;
		}
	}
})();