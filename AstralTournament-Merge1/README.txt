Documentazione per "The Astral Tournament"

CLASSI:

La classe Global è una classe singleton contenente tutti i valori necessari al fine del gioco (e quindi non devono essere distrutti quando si cambia scena), come ad esempio il veicolo e il personaggio scelto dal giocatori.
I valori memorizzati sono copie di quelli presenti sulla scena e sono figli di un gameObject temporaneo detto Group, ciò permette di evitare la creazione di infinite copie dell'oggetto selezionato.

Le classe Select è uno script che permette di scegliere i componenti per montare il veicolo, nell'apposita scena ve ne sono quattro istanze, una per ogni componente. I componenti selezionati vengono inseriti in una variabile statica di tipo Dictonary, essa verrà utilizzata per la costruzione effettiva del veicolo e per calcolare le relative statistiche.

La classe Vehicle è uno script che permette la costruzione effettiva del veicolo in base al contenuto della variabile Dictonary.

La classe Character definisce i personaggi utilizzabili nel gioco, nell'apposita schermata ve ne sono sei istanze. La classe contiene il nome del personaggio e una sua descrizione, inoltre vi è il tipo, utile per il calcolo del danni.

La classe VehicleComponent indica il singolo componente del veicolo, essa contiene una lista di float indicante le statistiche.

La classe Arena definisce l'arena in cui si affronteranno i giocatori.

La classe Type indica il tipo del personaggio scelto è influenza i danni in base al tipo di attacco ricevuto

SCRIPT:

Per il movimento del veicolo si utilizza la classe VehicleMovement
 