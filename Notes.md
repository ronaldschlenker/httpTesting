
# Einleitung

* Wir entwickeln http Services (REST, SOAP, etc.)
* Wie das so ist: Nichts ist super spezifiziert.
* Während der Entwicklung geht Wissen verloren.
* Darauf möchte ich einfehen

# Ablauf

## Vorstellung
* Server zeigen mit Person record und Datenbank und app

## "Wie sieht ein typischer Entwicklungsablauf aus?"

Browser: http://localhost:8080/persons
vscode http: http://localhost:8080/persons

* Personsn werden zurückgegeben
* NICHT sortiert.

Neue Anforderung (als Vereinbarung zwischen Entwicklern)!
* Eigentlich müsste das spezifiziert werden, aber die Entwickler nehmen es nicht so genau und haben keine Lust, Dokumente zu aktualisieren.

* sort einbauen und testen mit (vscode http)

## Ergebnis: Alles läuft! Prima.
ABER: Das Wissen ist verloren.

## fsx
Die Testabläuft, die vorher von Hand ausgeführt wurden, werden automatisiert.

## Übertrag in Test
"Oh, wie haben einen Integrationstest geschrieben!"
