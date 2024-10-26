# Dijkstra IP Routing

---

My C# solution to exercise from [Beej's Guide to Network Concepts - Project 6: Routing with Dijkstra’s](https://beej.us/guide/bgnet0/html/split/project-6-routing-with-dijkstras.html#project-6-routing-with-dijkstras)

Finds the shortest path between two given IP addresses using information from a list of routers and their connections. The shortest path is found using Dijkstra's single-source shortest paths algorithm.
The graph is constructed from a JSON file with the router network details. Each vertex represents a  router with its IP address as the vertex's name. The graph edges represent a connection between two routers.
Each vertex includes a property for the "administrative distance" (ad) which represents the weight of the connection. Another property is the subnet mask, which helps us describe the router's subnet.

## The JSON File with Router Information

Snippet from the JSON file used to construct the router's network graph:

```json
{
    "routers": {
        "10.34.98.1": {
            "connections": {
                "10.34.166.1": {
                    "netmask": "/24",
                    "interface": "en0",
                    "ad": 70
                },
                "10.34.194.1": {
                    "netmask": "/24",
                    "interface": "en1",
                    "ad": 93
                },
                "10.34.46.1": {
                    "netmask": "/24",
                    "interface": "en2",
                    "ad": 64
                }
            },
            "netmask": "/24",
            "if_count": 3,
            "if_prefix": "en"
        },
        "10.34.250.1": {
            "connections": {
                "10.34.52.1": {
                    "netmask": "/24",
                    "interface": "eth0",
                    "ad": 62
                },
                "10.34.166.1": {
                    "netmask": "/24",
                    "interface": "eth1",
                    "ad": 116
                }
            },
            "netmask": "/24",
            "if_count": 2,
            "if_prefix": "eth"
        }
    }
}
```
