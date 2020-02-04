SELECT AVG("count"), "artist" FROM (
	SELECT COUNT(1), artist, "FullName" FROM (
		SELECT u."FullName",ar."Title" artist, au."Title" audio
		FROM public."VkAudio" au
		JOIN "VkAudioToArtist" ata ON ata."VkAudioId" = au."Id"
		JOIN "VkAudioArtist" ar ON ar."Id" = ata."VkAudioArtistsId"
		JOIN "VkAudioToUser" atu ON atu."VkAudioId" = au."Id"
		JOIN "VkUsers" u ON u."Id" = atu."VkUserId"
		--WHERE LOWER(ar."Title") like '%stigma%'
		ORDER BY ar."Title", au."Title", u."FullName"
	) result
	GROUP BY artist, "FullName"
) result
WHERE "count" >1
GROUP BY "artist"
ORDER BY "artist" 
			   