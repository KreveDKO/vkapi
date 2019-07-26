﻿var loadedFriends = [];
var graph;
function onLoad() {
  graph = Viva.Graph.graph();
  var graphics = Viva.Graph.View.svgGraphics();

  var defs = Viva.Graph.svg('defs');
  graphics.getSvgRoot().append(defs);
  graphics.node(createNodeWithImage)
    .placeNode(placeNodeWithTransform);

  var renderer = Viva.Graph.View.renderer(graph, {
    graphics: graphics,
    container: document.getElementById('graph-container')
  });
  
  
  
  renderer.run();
  
  if (vkapi.setToken())
init();
}
  function createNodeWithImage(node) {
    var radius = 12;
    // First, we create a fill pattern and add it to SVG's defs:
    var pattern = Viva.Graph.svg('pattern')
      .attr('id', "imageFor_" + node.id)
      .attr('patternUnits', "userSpaceOnUse")
      .attr('width', 100)
      .attr('height', 100)
    var image = Viva.Graph.svg('image')
      .attr('x', '0')
      .attr('y', '0')
      .attr('height', radius * 2)
      .attr('width', radius * 2)
      .link(node.data.description);
    pattern.append(image);
    defs.append(pattern);

    // now create actual node and reference created fill pattern:
    var ui = Viva.Graph.svg('g');
    var circle = Viva.Graph.svg('circle')
      .attr('cx', radius)
      .attr('cy', radius)
      .attr('fill', 'url(#imageFor_' + node.id + ')')
      .attr('r', radius);
    $(ui).click(function () {
      if (node.data.isactive)
      loadFriends(node.id);
    });
    ui.append(circle);
    return ui;
  }

  function placeNodeWithTransform(nodeUI, pos) {
    // Shift image to let links go to the center:
    nodeUI.attr('transform', 'translate(' + (pos.x - 12) + ',' + (pos.y - 12) + ')');
  }

/** Загружает первую точку **/
function init() {
	vkapi.getSelf();
}
function loadFriends(userid) {
  if (loadedFriends.find(f => f === userid)) return;
  loadedFriends.push(userid);
  vkapi.getFriends(userid)
}