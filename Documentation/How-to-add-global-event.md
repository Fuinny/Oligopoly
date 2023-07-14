# :question: How to add your own global event
1. Open ```GlobalEvents.xml``` file.
2. Find ```GlobalEvent``` element.
3. Copy the following template and paste it below last ```GlobalEvent``` element:

   ```xml
	<GlobalEvent id="GLOBAL_EVENT_ID">
		<Effect>GLOBAL_EVENT_EFFECT</Effect>
		<Title>GLOBAL_EVENT_TITLE</Title>
		<Content>GLOBAL_EVENT_CONTENT</Content>
	</GlobalEvent>
   ```

5. Replace **GLOBAL_EVENT_ID**, **GLOBAL_EVENT_EFFECT**, **GLOBAL_EVENT_TITLE** and **GLOBAL_EVENT_CONTENT** with the appropriate values for your new event.
6. Save the changes to the ```GlobalEvents.xml``` file.

> **Please note:**
>
> **GLOBAL_EVENT_ID*** - can be anything, make it so that it is easier for you to navigate in the xml file, **but** you need to use underscore between words.
> The default structure is: Company_Event, but you can use other structure.
>
> Example: ```ABC_Company_new_event```
>
> **GLOBAL_EVENT_EFFECT** - can be any integer except zero.
>
> **GLOBAL_EVENT_TITLE** and **GLOBAL_EVENT_CONTENT** - can be anything, except null or whitespace.
>
> Example: ```ABC Company something happened``` and ```ABC Company long-long-long event content.```