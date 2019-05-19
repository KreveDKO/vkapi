function onLoad() {
  var graph = Viva.Graph.graph();
  var layout = Viva.Graph.Layout.forceDirected(graph, {
    springLength: 30,
    springCoeff: 0.0008,
    dragCoeff: 0.01,
    gravity: -1.2,
    theta: 1
  });

  var graphics = Viva.Graph.View.webglGraphics();

  var renderer = Viva.Graph.View.renderer(graph,
    {
      layout: layout,
      graphics: graphics,
      renderLinks: true,
      prerender: true
    });

  fetch("/response.json")
    .then(res => res.json())
    .then((out) => {
      console.log('Checkout this JSON! ', out);
      nodes = out.links;
      out.nodes.forEach(function (n) {
        graph.addNode(n.id, n);
      });
      out.links.forEach(function (n) {
        graph.addLink(n.source, n.target)
      }
      );
    })
    .catch(err => { throw err })
  renderer.run();
}