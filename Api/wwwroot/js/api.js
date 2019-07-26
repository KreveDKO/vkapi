var vkapi = {
  "token": 'access_token=',
  "version": 'v=5.95',
  "proxy": '',
  "apiUrl": 'https://api.vk.com/method/',
  "getSelf": function () {
    var result = {};
    var arguments = [this.version, this.token, 'fields=photo_50'];
    var uri = this.proxy + this.apiUrl + "users.get?" + arguments.join("&");
    console.log(uri);
    //fetch(uri)
    //  .then(res => res.json())
    //  .then((out) => {
    //    graph.addNode(out.id, result);
    //  });
    var settings = {
      
      "crossDomain": true,
      "url": uri,
      "method": "GET"
    }

    $.ajax(settings).done(function (response) {
      console.log(response);
    });
    return result;
  },
  "setToken": function () {

    var results = new RegExp('[\?&]' + 'access_token' + '=([^&#]*)').exec(window.location.href);
    if (results != null) {
      var cookieToken = decodeURI(results[1]) || 0;
      $.cookie("token", cookieToken, { expires: 30, path: '/' });
    }
    var token = $.cookie("token");
    if (token == null) {
      return false;
    }
    this.token += token;
    console.log(this.token);
    return true;

  },
  "getFriends": function (id) {
    var result = [];
    if (!id) return result;
    var arguments = [this.version, this.token, 'fields=photo_50', 'user_id=' + id];
    var uri = this.proxy + this.apiUrl + "users.get?" + arguments.join("&");
    fetch(uri)
      .then(res => res.json()).
      then((out) => {
        result = out.repsonse.items;
        result.forEach(function (n) {
          if (graph.nodes.find(f => f.id === n.id)) return;
          graph.addNode(n.id, n);
        });
        result.forEach(function (n) {
          graph.addLink(id, n.id);
        });
      });

  }
}